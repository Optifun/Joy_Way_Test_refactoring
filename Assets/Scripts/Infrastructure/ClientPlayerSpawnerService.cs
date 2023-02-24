using System;
using JoyWay.Infrastructure.Factories;
using Mirror;
using UnityEngine;
namespace JoyWay.Infrastructure
{
    public class ClientPlayerSpawnerService:IDisposable
    {
        private readonly CharacterFactory _factory;
        private readonly GameObject _playerPrefab;

        public ClientPlayerSpawnerService(GameObject playerPrefab, CharacterFactory factory)
        {
            _playerPrefab = playerPrefab;
            _factory = factory;
            NetworkClient.RegisterPrefab(playerPrefab, Spawn, Despawn);
        }

        private GameObject Spawn(SpawnMessage msg)
        {
            var characterContainer = _factory.CreateCharacter(msg.position, msg.rotation, msg.isOwner);
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
