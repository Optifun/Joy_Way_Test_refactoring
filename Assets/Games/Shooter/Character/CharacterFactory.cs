using JoyWay.Core.Infrastructure;
using JoyWay.Core.Services;
using JoyWay.Games.Shooter.StaticData;
using UnityEngine;
using Zenject;

namespace JoyWay.Games.Shooter.Character
{
    public class CharacterFactory 
    {
        private readonly DiContainer _diContainer;
        private readonly ILaunchContext _launchContext;
        private readonly PrefabSpawner _prefabSpawner;

        public CharacterFactory(DiContainer diContainer, ILaunchContext launchContext, PrefabSpawner prefabSpawner)
        {
            _prefabSpawner = prefabSpawner;
            _diContainer = diContainer;
            _launchContext = launchContext;
        }

        public CharacterContainer CreateCharacter(CharacterConfig characterConfig, CharacterContainer prefab, Vector3 position, Quaternion rotation, uint netId, bool isOwner)
        {
            bool isHost = _launchContext.IsHost;
            bool isClient = _launchContext.IsClient;

            var container = _prefabSpawner.Spawn(prefab, position, rotation);

            var interactionComponent = container.NetworkInteraction;
            var movementComponent = container.NetworkMovement;
            var lookComponent = container.NetworkLook;
            var healthComponent = container.Health;
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
                container.Character.SetupLocalPlayer();
                lookComponent.AttachCamera();
            }

            lookComponent.Setup(characterConfig.InterpolationTimeInterval);
            damageView.Setup(characterConfig.DisplayDamageTakenDelay);
            healthBar.Setup(Camera.main);
            healthBar.SetHealth(healthComponent.Health, healthComponent.MaxHealth);
            damageController.Construct(netId, healthBar, damageView);

            return container;
        }
    }
}
