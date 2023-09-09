using Core.Components;
using Core.UI;
using UnityEngine;

namespace JoyWay.Game.Character
{
    public class CharacterContainer : MonoBehaviour
    {
        [field: SerializeField] public NetworkCharacter Character { get; private set; }
        [field: SerializeField] public HealthNetworkComponent Health { get; private set; }
        [field: SerializeField] public NetworkCharacterMovementComponent NetworkMovement { get; private set; }
        [field: SerializeField] public NetworkCharacterShootingComponent NetworkShooting { get; private set; }
        [field: SerializeField] public NetworkCharacterInteractionComponent NetworkInteraction { get; private set; }
        [field: SerializeField] public NetworkCharacterLookComponent NetworkLook { get; private set; }
        [field: SerializeField] public CharacterHeadRotation HeadRotation { get; private set; }
        [field: SerializeField] public DamageView DamageView { get; private set; }
        [field: SerializeField] public HealthBarUI HealthBarUI { get; private set; }
    }
}