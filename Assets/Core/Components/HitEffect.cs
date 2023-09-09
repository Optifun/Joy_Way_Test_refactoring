using UnityEngine;
namespace Core.Components
{
    [System.Serializable]
    public abstract class HitEffect : ScriptableObject
    {
        public abstract void ApplyEffect(HealthNetworkComponent healthComponent);
    }
}