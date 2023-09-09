using System;
using JoyWay.Core.Messages;
using JoyWay.Core.Services;
using JoyWay.Games.Shooter.Character;
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
        private readonly IBufferedSubscriber<SpawnCharacterServerMessage> _characterSpawned;
        private ILaunchContext _launchContext;

        private IDisposable _subscription;

        public ServerPlayerSpawnerSystem(ILaunchContext launchContext, IBufferedSubscriber<SpawnCharacterServerMessage> characterSpawned,
            CharacterFactory factory, LevelSpawnPoints levelSpawnPoints)
        {
            _launchContext = launchContext;
            _characterSpawned = characterSpawned;
            _levelSpawnPoints = levelSpawnPoints;
            _factory = factory;
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }

        public void Initialize()
        {
            _subscription = _characterSpawned.Subscribe(SpawnCharacter);
        }

        private void SpawnCharacter(SpawnCharacterServerMessage message)
        {
            var spawnPoint = _levelSpawnPoints.GetRandomSpawnPoint();
            var character = CreateCharacterOnServer(spawnPoint.position, spawnPoint.rotation, message.Connection);
        }

        private CharacterContainer CreateCharacterOnServer(Vector3 position, Quaternion rotation, NetworkConnectionToClient conn)
        {
            bool isOwner = conn.identity.isOwned;
            var characterContainer = _factory.CreateCharacter(position, rotation, conn.identity.netId, isOwner);
            NetworkServer.Spawn(characterContainer.gameObject, conn);
            return characterContainer;
        }
    }
}
