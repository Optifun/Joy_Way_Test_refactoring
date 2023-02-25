using System;
using JoyWay.Game.Messages;
using MessagePipe;
using Mirror;

namespace JoyWay.Game.Character
{
    public class DamageController
    {
        private readonly ISubscriber<HealthUpdateMessage> _damageMessage;
        private HealthBarUI _healthBar;
        private DamageView _damageView;
        private uint _selfNetId;
        private IDisposable _subscription;

        public DamageController(ISubscriber<HealthUpdateMessage> damageMessage)
        {
            _damageMessage = damageMessage;
        }

        public void Construct(uint netId, HealthBarUI healthBar, DamageView damageView)
        {
            _healthBar = healthBar;
            _damageView = damageView;
            _selfNetId = netId;
            _subscription = _damageMessage.Subscribe(DamageReceived, message =>
                message.Target.netId == _selfNetId && message.Delta < 0);
        }

        private void DamageReceived(HealthUpdateMessage message)
        {
            _damageView.DisplayDamageTaken();
            _healthBar.SetHealth(message.UpdatedHealth, message.MaxHealth);
        }

        private void OnDestroy()
        {
            _subscription.Dispose();
        }
    }
}
