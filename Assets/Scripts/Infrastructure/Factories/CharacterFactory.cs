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
        private LaunchParameters _launchParameters;
        private DiContainer _diContainer;

        public CharacterFactory(DiContainer diContainer,LaunchParameters launchParameters, AssetContainer assetContainer)
        {
            _diContainer = diContainer;
            _launchParameters = launchParameters;
            _assetContainer = assetContainer;
            NetworkClient.RegisterPrefab(_assetContainer.Character.Value.gameObject, SpawnCharacterOnClient, UnspawnCharacterOnClient);
        }

        public CharacterContainer SpawnCharacterOnServer(Transform at, NetworkConnectionToClient conn)
        {
            bool isOwner = conn.identity.isOwned;
            var characterContainer = CreateCharacter(at.position, at.rotation, isOwner);
            NetworkServer.Spawn(characterContainer.gameObject, conn);
            return characterContainer;
        }

        private GameObject SpawnCharacterOnClient(SpawnMessage msg)
        {
            var characterContainer = CreateCharacter(msg.position, msg.rotation, msg.isOwner);
            return characterContainer.gameObject;
        }


        private void UnspawnCharacterOnClient(GameObject spawned)
        {
            Object.Destroy(spawned);
        }

        private CharacterContainer CreateCharacter(Vector3 position, Quaternion rotation, bool isOwner)
        {
            var isHost = _launchParameters.IsHost;
            var isClient = _launchParameters.IsClient;

            CharacterContainer container = Object.Instantiate(_assetContainer.Character.Value, position, rotation);
            _diContainer.InjectGameObject(container.gameObject);

            CharacterConfig characterConfig = _assetContainer.CharacterConfig.Value;
            var interactionComponent = container.NetworkInteraction;
            var movementComponent = container.NetworkMovement;
            var lookComponent = container.NetworkLook;
            var healthComponent = container.NetworkHealth;
            var damageView = container.DamageView;
            var healthBar = container.HealthBarUI;

            if (isHost)
            {
                healthComponent.Setup(characterConfig.MaxHealth);
                movementComponent.Setup(characterConfig.MaxSpeed, characterConfig.MovementForce, characterConfig.JumpForce,
                    characterConfig.GroundDrag, characterConfig.AirDrag);
            }

            if (isOwner)
            {
                interactionComponent.Setup(characterConfig.MaxInteractionDistance);
                container.NetworkCharacter.SetupLocalPlayer();
                lookComponent.AttachCamera();
            }

            lookComponent.Setup(characterConfig.InterpolationTimeInterval);
            damageView.Setup(characterConfig.DisplayDamageTakenDelay);
            healthBar.SetHealth(healthComponent.Health, healthComponent.MaxHealth);
            healthComponent.HealthChanged += OnHealthChanged;

            void OnHealthChanged(int currentHp, int maxHp)
            {
                damageView.DisplayDamageTaken();
                healthBar.SetHealth(currentHp, maxHp);
            }

            return container;
        }
    }
}