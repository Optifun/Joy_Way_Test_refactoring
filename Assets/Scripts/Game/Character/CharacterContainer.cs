using UnityEngine;

namespace JoyWay.Game.Character
{
    public class CharacterContainer : MonoBehaviour
    {
        [field: SerializeField] public NetworkCharacter NetworkCharacter { get; private set; }
        [field: SerializeField] public NetworkCharacterHealthComponent NetworkHealth { get; private set; }
        [field: SerializeField] public NetworkCharacterMovementComponent NetworkMovement { get; private set; }
        [field: SerializeField] public NetworkCharacterShootingComponent NetworkShooting { get; private set; }
        [field: SerializeField] public NetworkCharacterInteractionComponent NetworkInteraction { get; private set; }
        [field: SerializeField] public NetworkCharacterLookComponent NetworkLook { get; private set; }
        [field: SerializeField] public CharacterHeadRotation HeadRotation { get; private set; }
        [field: SerializeField] public DamageViewComponent DamageView { get; private set; }
        [field: SerializeField] public CharacterHealthBarUI HealthBarUI { get; private set; }
    }
}