﻿using JoyWay.Game.Character;
using UnityEngine;

namespace JoyWay.Game.Projectiles
{
    [CreateAssetMenu(menuName = "HitEffects/PeriodicalHit", fileName = "PeriodicalHit", order = 0)]
    public class PeriodicalHitEffect : InstantHitEffect
    {
        [SerializeField] private float _applingEffectDelay;
        [SerializeField] private int _numberOfTimes;
        [SerializeField] private int _periodicDamage;
        
        public override void ApplyEffect(HealthNetworkComponent healthComponent)
        {
            base.ApplyEffect(healthComponent);
            
            if (healthComponent.gameObject.TryGetComponent<PeriodicalDamageComponent>(out var periodicalDamage))
            {
                periodicalDamage.StopEffect();
            }
            else
            {
                periodicalDamage = healthComponent.gameObject.AddComponent<PeriodicalDamageComponent>();
            }

            periodicalDamage.Apply(healthComponent, _periodicDamage, _numberOfTimes, _applingEffectDelay);
        }

    }
}