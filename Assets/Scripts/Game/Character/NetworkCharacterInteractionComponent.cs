using JoyWay.Game.Projectiles;
using JoyWay.Services;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JoyWay.Game.Character
{
    public class NetworkCharacterInteractionComponent : NetworkBehaviour
    {
        [SerializeField] private Transform _handEndTransform;
        [SerializeField] private NetworkCharacterLookComponent _lookComponent;

        private InputService _inputService;
        private float _maxInteractionDistance;

        private PickableProjectile _objectInHand;

        public void Setup(float maxInteractionDistance)
        {
            _maxInteractionDistance = maxInteractionDistance;
        }

        public void Interact()
        {
            Transform cameraTransform = _lookComponent.GetCameraTransform();
            CmdHandleInteraction(cameraTransform.position, cameraTransform.forward);
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

            Transform hitTransform = GetRaycastHitTransform(position, direction);

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

            if (hitTransform.TryGetComponent<PickableProjectile>(out pickableProjectile))
                return true;
            else
                return false;
        }

        [Server]
        public bool TryGetInteractiveObject(Transform hitTransform, out IInteractable interactableObject)
        {
            interactableObject = null;

            if (hitTransform.TryGetComponent<IInteractable>(out interactableObject))
                return true;
            else
                return false;
        }

        [Server]
        private Transform GetRaycastHitTransform(Vector3 position, Vector3 direction)
        {
            Ray ray = new Ray(position, direction);
            RaycastHit raycastHit;
            Physics.Raycast(ray, out raycastHit, _maxInteractionDistance);
            Transform hitTransform = raycastHit.transform;
            return hitTransform;
        }
    }
}
