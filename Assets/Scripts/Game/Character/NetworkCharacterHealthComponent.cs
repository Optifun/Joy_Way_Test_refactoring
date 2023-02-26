using JoyWay.Game.Messages;
using MessagePipe;
using Mirror;
using Zenject;

namespace JoyWay.Game.Character
{
    public class NetworkCharacterHealthComponent : NetworkBehaviour
    {
        [field: SyncVar]
        public int MaxHealth { get; private set; }
        [field: SyncVar(hook = nameof(SetHealth))]
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
            Health -= damage;
        }

        private void SetHealth(int oldHealth, int newHealth)
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
