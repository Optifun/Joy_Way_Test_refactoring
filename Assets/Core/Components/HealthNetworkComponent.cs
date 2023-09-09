using System;
using Core.Messages;
using MessagePipe;
using Mirror;
using Zenject;
namespace Core.Components
{
    public class HealthNetworkComponent : NetworkBehaviour
    {
        [field: SyncVar]
        public int MaxHealth { get; private set; }
        [field: SyncVar(hook = nameof(OnSetHealth))]
        public int Health { get; private set; }

        private IPublisher<HealthUpdateMessage> _publisher;

        [Inject]
        private void Initialize(IPublisher<HealthUpdateMessage> publisher)
        {
            _publisher = publisher;
        }

        public void Setup(int maxHealth)
        {
            MaxHealth = maxHealth;
            Health = maxHealth;
        }

        [Server]
        public void ApplyDamage(int damage)
        {
            SetHealth(Health - damage);
        }

        private void SetHealth(int newHealth)
        {
            Health = Math.Clamp(newHealth, 0, MaxHealth);
        }

        private void OnSetHealth(int oldHealth, int newHealth)
        {
            int delta = newHealth - oldHealth;

            _publisher.Publish(new HealthUpdateMessage()
            {
                Target = netIdentity,
                MaxHealth = MaxHealth,
                UpdatedHealth = newHealth,
                Delta = delta
            });
        }
    }
}
