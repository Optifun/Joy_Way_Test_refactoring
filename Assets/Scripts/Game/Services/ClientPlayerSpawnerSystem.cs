using System;
using JoyWay.Infrastructure.Factories;
using Mirror;
using UnityEngine;
using Zenject;

namespace JoyWay.Infrastructure
{
    public class ClientPlayerSpawnerSystem : IInitializable, IDisposable
    {
        private readonly CharacterFactory _factory;
        private readonly GameObject _playerPrefab;

        public ClientPlayerSpawnerSystem(GameObject playerPrefab, CharacterFactory factory)
        {
            _playerPrefab = playerPrefab;
            _factory = factory;
        }

        public void Initialize()
        {
            NetworkClient.RegisterPrefab(_playerPrefab, Spawn, Despawn);
        }

        private GameObject Spawn(SpawnMessage msg)
        {
            var characterContainer = _factory.CreateCharacter(msg.position, msg.rotation, msg.netId, msg.isOwner);
            return characterContainer.gameObject;
        }

        private void Despawn(GameObject player)
        {
            GameObject.Destroy(player);
        }

        public void Dispose()
        {
            NetworkClient.UnregisterPrefab(_playerPrefab);
        }
    }
}
