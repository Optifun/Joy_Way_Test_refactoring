using System;
using JoyWay.Core.Infrastructure.AssetManagement;
using JoyWay.Games.Shooter.Character;
using JoyWay.Games.Shooter.StaticData;
using Mirror;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
namespace JoyWay.Games.Shooter.Services
{
    public class ClientPlayerSpawnerSystem : IInitializable, IDisposable
    {
        private readonly CharacterFactory _factory;
        private readonly CharacterConfig _playerConfig;
        private readonly IAssets _assets;
        private CharacterContainer _characterPrefab;

        public ClientPlayerSpawnerSystem(CharacterConfig playerConfig, CharacterFactory factory, IAssets assets)
        {
            _assets = assets;
            _playerConfig = playerConfig;
            _factory = factory;
        }

        public async void Initialize()
        {
            var loadedPrefab = await _assets.Load<GameObject>(_playerConfig.Prefab);
            _characterPrefab = loadedPrefab.GetComponent<CharacterContainer>();
            NetworkClient.UnregisterPrefab(loadedPrefab);
            NetworkClient.RegisterPrefab(loadedPrefab, Spawn, Despawn);
        }
        public void Dispose()
        {
            NetworkClient.UnregisterPrefab(_characterPrefab.gameObject);
        }

        private GameObject Spawn(SpawnMessage msg)
        {
            var characterContainer = _factory.CreateCharacter(_playerConfig, _characterPrefab, msg.position, msg.rotation, msg.netId, msg.isOwner);
            return characterContainer.gameObject;
        }

        private void Despawn(GameObject player)
        {
            Object.Destroy(player);
        }
    }
}
