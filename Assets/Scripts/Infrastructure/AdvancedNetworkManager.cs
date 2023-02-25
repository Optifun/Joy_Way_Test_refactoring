using System;
using System.Collections;
using System.Net;
using JoyWay.Game.Messages;
using JoyWay.Resources;
using JoyWay.Services;
using JoyWay.UI;
using kcp2k;
using MessagePipe;
using Mirror;
using UnityEngine;
using Zenject;

namespace JoyWay.Infrastructure
{
    public class AdvancedNetworkManager : NetworkManager, ICoroutineRunner
    {
        public new static AdvancedNetworkManager singleton { get; private set; }

        public event Action Connected;
        public event Action Disconnected;

        public bool IsClient { get; private set; }
        public bool IsServer { get; private set; }


        private IBufferedPublisher<SpawnCharacterServerMessage> _spawnCharacter;

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

        public void Connect(IPAddress ipAddress)
        {
            if (transport is KcpTransport kcpTransport == false)
            {
                throw new ArgumentException("Not supported transport");
            }

            IsClient = true;
            UriBuilder builder = new UriBuilder();
            var exampleUri = transport.ServerUri();
            builder.Scheme = exampleUri.Scheme;
            builder.Port = exampleUri.Port;
            builder.Host = ipAddress.ToString();
            StartClient(builder.Uri);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            IsClient = true;
            Connected?.Invoke();
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            IsClient = false;
            Disconnected?.Invoke();
        }

        public override void OnStartHost()
        {
            base.OnStartHost();
            IsServer = true;
            // Connected?.Invoke();
        }

        public override void OnStopHost()
        {
            base.OnStopHost();
            IsServer = false;
            // Disconnected?.Invoke();
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            GameObject player = Instantiate(playerPrefab);
            player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
            NetworkServer.AddPlayerForConnection(conn, player);

            _spawnCharacter.Publish(new SpawnCharacterServerMessage() { Connection = conn });
        }
    }
}
