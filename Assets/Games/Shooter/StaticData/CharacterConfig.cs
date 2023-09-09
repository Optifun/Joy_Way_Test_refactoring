using UnityEngine;

namespace JoyWay.Game.Character
{
    [CreateAssetMenu(menuName = "Character/CharacterConfig", fileName = "CharacterConfig", order = 0)]
    public class CharacterConfig : ScriptableObject
    {
        [field: Header("Health Component")]
        [field: SerializeField] public int MaxHealth { get; private set; }

        [field: Header("Movement Component")]
        [field: SerializeField] public int MaxSpeed { get; private set; }
        [field: SerializeField] public int MovementForce { get; private set; }
        [field: SerializeField] public int JumpForce { get; private set; }
        [field: SerializeField] public int GroundDrag { get; private set; }
        [field: SerializeField] public int AirDrag { get; private set; }
        
        [field: Header("Interaction Component")]
        [field: SerializeField] public float MaxInteractionDistance { get; private set; }
        
        [field: Header("Look Component")]
        [field: SerializeField] public float InterpolationTimeInterval { get; private set; }
        
        [field: Header("View Component")]
        [field: SerializeField] public float DisplayDamageTakenDelay { get; private set; }
    }
}