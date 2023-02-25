using System;
using JoyWay.Game.Messages;
using JoyWay.Infrastructure;
using MessagePipe;
using Mirror;
using UnityEngine;
using Zenject;

namespace JoyWay.Game.Character
{
    public class NetworkCharacterHealthComponent : NetworkBehaviour
    {
        [SyncVar]
        private int _maxHealth;

        [SyncVar(hook = nameof(SetHealth))]
        private int _health;
        private IPublisher<HealthUpdateMessage> _publisher;

        public int MaxHealth => _maxHealth;
        public int Health => _health;

        [Inject]
        private void Initialize(IPublisher<HealthUpdateMessage> publisher)
        {
            _publisher = publisher;
        }

        public void Setup(int maxHealth)
        {
            _maxHealth = maxHealth;
            _health = maxHealth;
        }

        [Server]
        public void ApplyDamage(int damage)
        {
            _health -= damage;
        }

        private void SetHealth(int oldHealth, int newHealth)
        {
            int delta = newHealth - oldHealth;

            _publisher.Publish(new HealthUpdateMessage()
            {
                Target = netIdentity,
                MaxHealth = _maxHealth,
                UpdatedHealth = newHealth,
                Delta = delta
            });
        }
    }
}
