using System;
using System.Net;
using JoyWay.Game;
using JoyWay.Game.Character;
using JoyWay.Infrastructure.Factories;
using kcp2k;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace JoyWay.Infrastructure
{
    public class AdvancedNetworkManager : NetworkManager
    {
        public new static AdvancedNetworkManager singleton { get; private set; }
        
        public event Action Connected;
        public event Action Disconnected;
        
        private LevelSpawnPoints _levelSpawnPoints;
        private CharacterFactory _characterFactory;
        public bool IsClient { get; private set; }
        public bool IsServer { get; private set; }

        [Inject]
        public void Construct(CharacterFactory characterFactory)
        {
            _characterFactory = characterFactory;
        }
        
        public override void Awake()
        {
            base.Awake();
            singleton = this;
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
            Connected?.Invoke();
        }

        public override void OnStopHost()
        {
            base.OnStopHost();
            IsServer = false;
            Disconnected?.Invoke();
        }

        private void RespawnCharacterOnServer(NetworkCharacterHealthComponent networkCharacterHealthComponent)
        {
            networkCharacterHealthComponent.Died -= RespawnCharacterOnServer;
            var conn = networkCharacterHealthComponent.netIdentity.connectionToClient;
            NetworkServer.Destroy(networkCharacterHealthComponent.gameObject);
            SpawnCharacterOnServer(conn);
        }

        private void SpawnCharacterOnServer(NetworkConnectionToClient conn)
        {
            Transform spawnPoint = GetRandomSpawnPoint();
            var character = _characterFactory.SpawnCharacterOnServer(spawnPoint, conn);
            var characterHealth = character.NetworkHealth;
            characterHealth.Died += RespawnCharacterOnServer;
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            SpawnCharacterOnServer(conn);
        }

        private Transform GetRandomSpawnPoint()
        {
            if (_levelSpawnPoints == null)
                _levelSpawnPoints = FindObjectOfType<LevelSpawnPoints>();

            return _levelSpawnPoints.GetRandomSpawnPoint();
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
    }
}
