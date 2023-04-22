using System;
using System.Net;
using Cysharp.Threading.Tasks;
using JoyWay.Game.Messages;
using JoyWay.UI;
using kcp2k;
using MessagePipe;
using Mirror;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace JoyWay.Infrastructure
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

        private IBufferedPublisher<SpawnCharacterServerMessage> _spawnCharacter;
        private UniTaskCompletionSource _completionSource;

        [Inject]
        public void Construct(IBufferedPublisher<SpawnCharacterServerMessage> spawnCharacter)
        {
            _spawnCharacter = spawnCharacter;
        }

        public override void Awake()
        {
            base.Awake();
            singleton = this;
        }

        public UniTask ConnectAsync(IPAddress ipAddress)
        {
            Assert.IsNull(_completionSource, "Some operation is in progress, can't Connect");
            _completionSource = new UniTaskCompletionSource();
            Connect(ipAddress);
            return _completionSource.Task;
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
            Assert.IsNull(_completionSource, "Some operation is in progress, can't start host");
            _completionSource = new UniTaskCompletionSource();
            StartHost();
            return _completionSource.Task;
        }


        public override void OnStartClient()
        {
            base.OnStartClient();
            IsClient = true;
            ClientConnected?.Invoke();
            _completionSource?.TrySetResult();
            _completionSource = null;
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            IsClient = false;
            ClientDisconnected?.Invoke();
        }

        public override void OnStartHost()
        {
            base.OnStartHost();
            IsServer = true;
            ServerStarted?.Invoke();
            _completionSource.TrySetResult();
            _completionSource = null;
        }

        public override void OnStopHost()
        {
            base.OnStopHost();
            IsServer = false;
            ServerStopped?.Invoke();
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            GameObject player = Instantiate(playerPrefab);
            player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
            NetworkServer.AddPlayerForConnection(conn, player);

            _spawnCharacter.Publish(new SpawnCharacterServerMessage() { Connection = conn });
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
