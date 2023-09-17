using System;
using JoyWay.Core.Infrastructure.AssetManagement;
using JoyWay.Core.Messages;
using JoyWay.Core.Services;
using JoyWay.Core.Utils;
using JoyWay.Games.Shooter.Character;
using JoyWay.Games.Shooter.StaticData;
using MessagePipe;
using Mirror;
using UnityEngine;
using Zenject;
namespace JoyWay.Games.Shooter.Services
{
    public class ServerPlayerSpawnerSystem : IInitializable, IDisposable
    {
        private readonly CharacterFactory _factory;
        private readonly LevelSpawnPoints _levelSpawnPoints;
        private readonly IBufferedSubscriber<SpawnPlayerServerMessage> _characterSpawned;
        private readonly CharacterConfig _characterConfig;
        private readonly IAssets _assets;

        private IDisposable _subscription;
        private CharacterContainer _characterPrefab;

        public ServerPlayerSpawnerSystem(IBufferedSubscriber<SpawnPlayerServerMessage> characterSpawned,
            CharacterFactory factory, CharacterConfig characterConfig, IAssets assets, LevelSpawnPoints levelSpawnPoints)
        {
            _assets = assets;
            _characterConfig = characterConfig;
            _characterSpawned = characterSpawned;
            _levelSpawnPoints = levelSpawnPoints;
            _factory = factory;
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }

        public async void Initialize()
        {
            var loadedAsset = await _assets.Load<GameObject>(_characterConfig.Prefab);
            _characterPrefab = loadedAsset.GetComponent<CharacterContainer>();
            _subscription = _characterSpawned.Subscribe(SpawnCharacter);
        }

        private void SpawnCharacter(SpawnPlayerServerMessage message)
        {
            var spawnPoint = _levelSpawnPoints.GetRandomSpawnPoint();
            var character = CreateCharacterOnServer(spawnPoint.position, spawnPoint.rotation, message.Connection);
        }

        private CharacterContainer CreateCharacterOnServer(Vector3 position, Quaternion rotation, NetworkConnectionToClient conn)
        {
            bool isOwner = conn.identity.isOwned;
            var characterContainer = _factory.CreateCharacter(_characterConfig, _characterPrefab, position, rotation, conn.identity.netId, isOwner);
            NetworkServer.Spawn(characterContainer.gameObject, conn);
            return characterContainer;
        }
    }
}
