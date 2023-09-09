using System;
using JoyWay.Core.Messages;
using JoyWay.Core.UI;
using MessagePipe;
using UnityEngine;
namespace JoyWay.Games.Shooter.Character
{
    public class DamageController : IDisposable
    {
        private readonly ISubscriber<HealthUpdateMessage> _damageMessage;
        private DamageView _damageView;
        private HealthBarUI _healthBar;
        private uint _selfNetId;
        private IDisposable _subscription;

        public DamageController(ISubscriber<HealthUpdateMessage> damageMessage)
        {
            _damageMessage = damageMessage;
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }

        public void Construct(uint netId, HealthBarUI healthBar, DamageView damageView)
        {
            _healthBar = healthBar;
            _damageView = damageView;
            _selfNetId = netId;

            var builder = DisposableBag.CreateBuilder(2);
            _damageMessage.Subscribe(HealthUpdated).AddTo(builder);
            _damageMessage.Subscribe(DamageReceived, message =>
                               message.Target.netId == _selfNetId && message.Delta < 0)
                       .AddTo(builder);
            _subscription = builder.Build();
        }

        private void HealthUpdated(HealthUpdateMessage message)
        {
            Debug.Log(message);
            _healthBar.SetHealth(message.UpdatedHealth, message.MaxHealth);
        }

        private void DamageReceived(HealthUpdateMessage message)
        {
            _damageView.DisplayDamageTaken();
        }
    }
}
