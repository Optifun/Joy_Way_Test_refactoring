using System;
using JoyWay.Game;
using JoyWay.Game.Character;
using JoyWay.Game.Messages;
using JoyWay.Infrastructure;
using JoyWay.Infrastructure.Factories;
using MessagePipe;
using Mirror;
using UnityEngine;
namespace JoyWay.Services
{
    public class ServerPlayerSpawnerService: IDisposable
    {
        private readonly CharacterFactory _factory;
        private readonly LevelSpawnPoints _levelSpawnPoints;

        private readonly IPublisher<SpawnCharacterServerMessage> _publisher;
        private readonly IDisposable _subscription;
        public ServerPlayerSpawnerService(
            ISubscriber<SpawnCharacterServerMessage> subscriber,
            IPublisher<SpawnCharacterServerMessage> publisher,
            CharacterFactory factory, LevelSpawnPoints levelSpawnPoints)
        {
            _publisher = publisher;
            _levelSpawnPoints = levelSpawnPoints;
            _factory = factory;
            _subscription = subscriber.Subscribe(SpawnCharacter);

        }

        private void SpawnCharacter(SpawnCharacterServerMessage message)
        {
            Transform spawnPoint = _levelSpawnPoints.GetRandomSpawnPoint();
            var character = CreateCharacterOnServer(spawnPoint.position, spawnPoint.rotation, message.Connection);
            character.NetworkHealth.Died += RespawnCharacter;
        }

        private void RespawnCharacter(NetworkCharacterHealthComponent healthComponent)
        {
            healthComponent.Died -= RespawnCharacter;
            var conn = healthComponent.netIdentity.connectionToClient;
            NetworkServer.Destroy(healthComponent.gameObject);
            _publisher.Publish(new SpawnCharacterServerMessage(){Connection = conn});
        }

        private CharacterContainer CreateCharacterOnServer(Vector3 position, Quaternion rotation, NetworkConnectionToClient conn)
        {
            bool isOwner = conn.identity.isOwned;
            var characterContainer = _factory.CreateCharacter(position, rotation, isOwner);
            NetworkServer.Spawn(characterContainer.gameObject, conn);
            return characterContainer;
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}
