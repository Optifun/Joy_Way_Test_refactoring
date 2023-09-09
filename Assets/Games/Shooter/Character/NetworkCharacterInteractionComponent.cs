using JoyWay.Core.Components;
using JoyWay.Core.Services;
using JoyWay.Games.Shooter.Projectiles;
using JoyWay.Games.Shooter.Services;
using Mirror;
using UnityEngine;
using Zenject;
namespace JoyWay.Games.Shooter.Character
{
    public class NetworkCharacterInteractionComponent : NetworkBehaviour
    {
        [SerializeField] private Transform _handEndTransform;
        private Transform _cameraTransform;

        private InputService _inputService;
        private float _maxInteractionDistance;

        private PickableProjectile _objectInHand;

        [Inject]
        private void Initialize(FPSCameraService fpsCameraService)
        {
            _cameraTransform = fpsCameraService.GetCameraTransform();
        }

        public void Setup(float maxInteractionDistance)
        {
            _maxInteractionDistance = maxInteractionDistance;
        }

        public void Interact()
        {
            CmdHandleInteraction(_cameraTransform.position, _cameraTransform.forward);
        }

        [Command]
        private void CmdHandleInteraction(Vector3 position, Vector3 direction)
        {
            if (_objectInHand != null)
            {
                _objectInHand.Throw(direction, netIdentity.netId);
                _objectInHand = null;
                return;
            }

            var hitTransform = GetRaycastHitTransform(position, direction);

            if (hitTransform == null)
                return;

            if (TryPickupObject(hitTransform, out var pickableObject))
            {
                if (pickableObject.CanPick)
                {
                    pickableObject.Pickup(_handEndTransform);
                    _objectInHand = pickableObject;
                }
                return;
            }

            if (TryGetInteractiveObject(hitTransform, out var interactableObject))
            {
                interactableObject.Interact();
            }
        }

        [Server]
        private bool TryPickupObject(Transform hitTransform, out PickableProjectile pickableProjectile)
        {
            pickableProjectile = null;

            if (hitTransform.TryGetComponent(out pickableProjectile))
                return true;
            return false;
        }

        [Server]
        public bool TryGetInteractiveObject(Transform hitTransform, out IInteractable interactableObject)
        {
            interactableObject = null;

            if (hitTransform.TryGetComponent(out interactableObject))
                return true;
            return false;
        }

        [Server]
        private Transform GetRaycastHitTransform(Vector3 position, Vector3 direction)
        {
            var ray = new Ray(position, direction);
            RaycastHit raycastHit;
            Physics.Raycast(ray, out raycastHit, _maxInteractionDistance);
            var hitTransform = raycastHit.transform;
            return hitTransform;
        }
    }
}
