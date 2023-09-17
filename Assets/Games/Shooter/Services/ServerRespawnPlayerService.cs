using System;
using JoyWay.Core.Messages;
using JoyWay.Games.Shooter.Character;
using MessagePipe;
using Mirror;
using Zenject;
namespace JoyWay.Games.Shooter.Services
{
    public class ServerRespawnPlayerService : IInitializable, IDisposable
    {
        private readonly ISubscriber<DeathMessage> _deathMessage;
        private readonly IPublisher<SpawnPlayerServerMessage> _spawnCharacter;
        private IDisposable _subscription;

        public ServerRespawnPlayerService(ISubscriber<DeathMessage> deathMessage,
            IPublisher<SpawnPlayerServerMessage> spawnCharacter)
        {
            _deathMessage = deathMessage;
            _spawnCharacter = spawnCharacter;
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }

        public void Initialize()
        {
            _subscription = _deathMessage.Subscribe(RespawnCharacter, message =>
                message.Target.gameObject.GetComponent<NetworkCharacter>() != null);
        }

        private void RespawnCharacter(DeathMessage message)
        {
            NetworkServer.Destroy(message.Target.gameObject);
            _spawnCharacter.Publish(new SpawnPlayerServerMessage(message.Target.connectionToClient));
        }
    }
}
