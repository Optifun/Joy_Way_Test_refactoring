using JoyWay.Game.Character;
using UnityEngine;

namespace JoyWay.Game.Projectiles
{
    [System.Serializable]
    public abstract class HitEffect : ScriptableObject
    {
        public abstract void ApplyEffect(HealthNetworkComponent healthComponent);
    }
}