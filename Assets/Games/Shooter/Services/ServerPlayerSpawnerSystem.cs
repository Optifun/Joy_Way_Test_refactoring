using System;
using Core.Messages;
using JoyWay.Game.Character;
using JoyWay.Infrastructure;
using JoyWay.Infrastructure.Factories;
using MessagePipe;
using Mirror;
using UnityEngine;
using Zenject;

namespace JoyWay.Game.Services
{
    public class ServerPlayerSpawnerSystem :IInitializable, IDisposable
    {
        private readonly CharacterFactory _factory;
        private readonly LevelSpawnPoints _levelSpawnPoints;

        private IDisposable _subscription;
        private IBufferedSubscriber<SpawnCharacterServerMessage> _characterSpawned;
        private ILaunchContext _launchContext;

        public ServerPlayerSpawnerSystem(ILaunchContext launchContext, IBufferedSubscriber<SpawnCharacterServerMessage> characterSpawned, 
            CharacterFactory factory, LevelSpawnPoints levelSpawnPoints)
        {
            _launchContext = launchContext;
            _characterSpawned = characterSpawned;
            _levelSpawnPoints = levelSpawnPoints;
            _factory = factory;
        }

        public void Initialize()
        {
            _subscription = _characterSpawned.Subscribe(SpawnCharacter);
        }

        private void SpawnCharacter(SpawnCharacterServerMessage message)
        {
            Transform spawnPoint = _levelSpawnPoints.GetRandomSpawnPoint();
            var character = CreateCharacterOnServer(spawnPoint.position, spawnPoint.rotation, message.Connection);
        }

        private CharacterContainer CreateCharacterOnServer(Vector3 position, Quaternion rotation, NetworkConnectionToClient conn)
        {
            bool isOwner = conn.identity.isOwned;
            var characterContainer = _factory.CreateCharacter(position, rotation, conn.identity.netId, isOwner);
            NetworkServer.Spawn(characterContainer.gameObject, conn);
            return characterContainer;
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}
