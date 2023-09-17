using System;
using System.Net;
using Cysharp.Threading.Tasks;
using JoyWay.Core.Messages;
using JoyWay.Core.Services;
using kcp2k;
using MessagePipe;
using Mirror;
using UnityEngine;
using Zenject;

namespace JoyWay.Core.Components
{
    public class AdvancedNetworkManager : NetworkManager, ICoroutineRunner
    {
        public new static AdvancedNetworkManager singleton { get; private set; }

        public event Action ServerStarted;
        public event Action ServerStopped;
        public event Action ClientConnected;
        public event Action ClientDisconnected;

        public bool IsClient { get; private set; }
        public bool IsServer { get; private set; }


        private IBufferedPublisher<SpawnPlayerServerMessage> _spawnPlayer;
        private IBufferedPublisher<NetworkPlayerSpawnedMessage> _playerSpawned;
        private IBufferedPublisher<ClientConnected> _clientConnected;
        private IBufferedPublisher<ClientDisconnected> _clientDisconnected;
        private IBufferedPublisher<ClientError> _clientError;
        
        private IBufferedPublisher<ServerClientConnected> _serverClientConnected;
        private IBufferedPublisher<ServerClientDisconnected> _serverClientDisconnected;
        private IBufferedPublisher<ServerError> _serverError;

        private UniTaskCompletionSource _serverConnectedCompletionSource;
        private UniTaskCompletionSource _clientConnectedCompletionSource;

        [Inject]
        public void Construct(
            IBufferedPublisher<SpawnPlayerServerMessage> spawnCharacter,
            IBufferedPublisher<NetworkPlayerSpawnedMessage> playerSpawned, 
            IBufferedPublisher<ClientConnected> clientConnected, 
            IBufferedPublisher<ClientDisconnected> clientDisconnected,
            IBufferedPublisher<ClientError> clientError,
            IBufferedPublisher<ServerClientConnected> serverClientConnected,
            IBufferedPublisher<ServerClientDisconnected> serverClientDisconnected,
            IBufferedPublisher<ServerError> serverError
            )
        {
            _serverError = serverError;
            _serverClientDisconnected = serverClientDisconnected;
            _serverClientConnected = serverClientConnected;
            _clientError = clientError;
            _clientDisconnected = clientDisconnected;
            _clientConnected = clientConnected;
            _spawnPlayer = spawnCharacter;
            _playerSpawned = playerSpawned;
        }

        public override void Awake()
        {
            base.Awake();
            singleton = this;
        }

        public UniTask ConnectAsync(IPAddress ipAddress)
        {
            _clientConnectedCompletionSource = new UniTaskCompletionSource();
            Connect(ipAddress);
            return _clientConnectedCompletionSource.Task;
        }

        public void Connect(IPAddress ipAddress)
        {
            if (transport is KcpTransport kcpTransport == false)
            {
                throw new ArgumentException("Not supported transport");
            }

            IsClient = true;
            var address = BuildURL(ipAddress, transport.ServerUri());
            StartClient(address);
        }
        
        public UniTask StartHostAsync()
        {
            _serverConnectedCompletionSource = new UniTaskCompletionSource();
            StartHost();
            return _serverConnectedCompletionSource.Task;
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            IsClient = true;
            ClientConnected?.Invoke();
            _clientConnectedCompletionSource?.TrySetResult();
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            IsClient = false;
            ClientDisconnected?.Invoke();
        }

        public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
        {
            base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);
        }

        public override void OnClientSceneChanged()
        {
            base.OnClientSceneChanged();
        }

        public override void OnStartHost()
        {
            base.OnStartHost();
            IsServer = true;
            ServerStarted?.Invoke();
            _serverConnectedCompletionSource.TrySetResult();
        }

        public override void OnStopHost()
        {
            base.OnStopHost();
            IsServer = false;
            ServerStopped?.Invoke();
        }


        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            _serverClientConnected.Publish(new ServerClientConnected(conn));
            
        }
        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            _serverClientDisconnected.Publish(new ServerClientDisconnected(conn));
            base.OnServerDisconnect(conn);
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            GameObject player = Instantiate(playerPrefab);
            player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
            NetworkServer.AddPlayerForConnection(conn, player);
            NetworkPlayer networkPlayer = player.GetComponent<NetworkPlayer>();

            _playerSpawned.Publish(new NetworkPlayerSpawnedMessage(conn, networkPlayer));
            _spawnPlayer.Publish(new SpawnPlayerServerMessage(conn));
        }

        public override void OnServerError(NetworkConnectionToClient conn, TransportError error, string reason)
        {
            base.OnServerError(conn, error, reason);
            _serverError.Publish(new ServerError(conn, error, reason));
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            _clientConnected.Publish(new ClientConnected(true));
        }

        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            _clientDisconnected.Publish(new ClientDisconnected(true));
        }

        public override void OnClientError(TransportError error, string reason)
        {
            base.OnClientError(error, reason);
            _clientError.Publish(new ClientError(error, reason));
        }

        private static Uri BuildURL(IPAddress ipAddress, Uri exampleUri)
        {
            UriBuilder builder = new UriBuilder
            {
                Scheme = exampleUri.Scheme,
                Port = exampleUri.Port,
                Host = ipAddress.ToString()
            };
            return builder.Uri;
        }
    }
}
