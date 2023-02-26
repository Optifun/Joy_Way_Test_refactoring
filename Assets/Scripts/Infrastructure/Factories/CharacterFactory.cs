using JoyWay.Game.Character;
using JoyWay.Services;
using Mirror;
using UnityEngine;
using Zenject;

namespace JoyWay.Infrastructure.Factories
{
    public class CharacterFactory
    {
        private readonly AssetContainer _assetContainer;
        private readonly ILaunchContext _launchContext;
        private readonly DiContainer _diContainer;

        public CharacterFactory(DiContainer diContainer, ILaunchContext launchContext, AssetContainer assetContainer)
        {
            _diContainer = diContainer;
            _launchContext = launchContext;
            _assetContainer = assetContainer;
        }

        public CharacterContainer CreateCharacter(Vector3 position, Quaternion rotation, uint netId, bool isOwner)
        {
            var isHost = _launchContext.IsHost;
            var isClient = _launchContext.IsClient;

            CharacterContainer container = Object.Instantiate(_assetContainer.Character.Value, position, rotation);
            _diContainer.InjectGameObject(container.gameObject);

            CharacterConfig characterConfig = _assetContainer.CharacterConfig.Value;
            var interactionComponent = container.NetworkInteraction;
            var movementComponent = container.NetworkMovement;
            var lookComponent = container.NetworkLook;
            var healthComponent = container.NetworkHealth;
            var damageView = container.DamageView;
            var healthBar = container.HealthBarUI;
            var damageController = _diContainer.Resolve<DamageController>();

            if (isHost)
            {
                interactionComponent.Setup(characterConfig.MaxInteractionDistance);
                healthComponent.Setup(characterConfig.MaxHealth);
                movementComponent.Setup(characterConfig.MaxSpeed, characterConfig.MovementForce, characterConfig.JumpForce,
                    characterConfig.GroundDrag, characterConfig.AirDrag);
            }

            if (isOwner)
            {
                container.NetworkCharacter.SetupLocalPlayer();
                lookComponent.AttachCamera();
            }

            lookComponent.Setup(characterConfig.InterpolationTimeInterval);
            damageView.Setup(characterConfig.DisplayDamageTakenDelay);
            healthBar.SetHealth(healthComponent.Health, healthComponent.MaxHealth);
            damageController.Construct(netId, healthBar, damageView);

            return container;
        }
    }
}