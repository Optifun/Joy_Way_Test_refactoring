using System;
using JoyWay.Core.Messages;
using MessagePipe;
using Zenject;
namespace JoyWay.Games.Shooter.Services
{
    public class CharacterDeathSystem : IInitializable, IDisposable
    {
        private readonly IPublisher<DeathMessage> _death;
        private readonly ISubscriber<HealthUpdateMessage> _healthUpdated;
        private IDisposable _subscription;

        public CharacterDeathSystem(ISubscriber<HealthUpdateMessage> healthUpdated, IPublisher<DeathMessage> death)
        {
            _death = death;
            _healthUpdated = healthUpdated;
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
        public void Initialize()
        {
            _subscription = _healthUpdated.Subscribe(CheckForDeath);
        }

        private void CheckForDeath(HealthUpdateMessage message)
        {
            if (message.UpdatedHealth <= 0)
            {
                _death.Publish(new DeathMessage
                {
                    Target = message.Target
                });
            }
        }
    }
}
