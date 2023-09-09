using System;
using Core.Messages;
using MessagePipe;
using Zenject;
namespace JoyWay.Game.Services
{
    public class CharacterDeathSystem : IInitializable, IDisposable
    {
        private readonly ISubscriber<HealthUpdateMessage> _healthUpdated;
        private readonly IPublisher<DeathMessage> _death;
        private IDisposable _subscription;

        public CharacterDeathSystem(ISubscriber<HealthUpdateMessage> healthUpdated, IPublisher<DeathMessage> death)
        {
            _death = death;
            _healthUpdated = healthUpdated;
        }
        public void Initialize()
        {
            _subscription = _healthUpdated.Subscribe(CheckForDeath);
        }

        private void CheckForDeath(HealthUpdateMessage message)
        {
            if (message.UpdatedHealth <= 0)
            {
                _death.Publish(new DeathMessage()
                {
                    Target = message.Target
                });
            }
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}
