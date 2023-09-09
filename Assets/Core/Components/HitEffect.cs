using UnityEngine;
namespace JoyWay.Core.Components
{
    [System.Serializable]
    public abstract class HitEffect : ScriptableObject
    {
        public abstract void ApplyEffect(HealthNetworkComponent healthComponent);
    }
}