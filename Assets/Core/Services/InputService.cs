using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Core.Services
{
    public class InputService : MonoBehaviour, IDisposable
    {
        public event Action<Vector2> Move;
        public event Action Jump;
        public event Action Fire;
        public event Action Interact;

        private PlayerInputs _playerInputs;
        private Vector2 _moveDirection;
        private bool _initialized = false;

        [Inject]
        public void Initialize(PlayerInputs playerInputs)
        {
            _playerInputs = playerInputs;
            _playerInputs.Character.Jump.performed += OnJump;
            _playerInputs.Character.Fire.performed += OnFire;
            _playerInputs.Character.Interact.performed += OnInteract;
            _initialized = true;
        }

        private void FixedUpdate()
        {
            if (false == _initialized)
                return;

            _moveDirection = _playerInputs.Character.Move.ReadValue<Vector2>();
            Move?.Invoke(_moveDirection);
        }

        public void Enable()
        {
            _playerInputs.Enable();
        }

        public void Disable()
        {
            _playerInputs.Disable();
        }

        private void OnInteract(InputAction.CallbackContext x) => Interact?.Invoke();
        private void OnFire(InputAction.CallbackContext x) => Fire?.Invoke();
        private void OnJump(InputAction.CallbackContext x) => Jump?.Invoke();

        public void Dispose()
        {
            _playerInputs.Character.Jump.performed -= OnJump;
            _playerInputs.Character.Fire.performed -= OnFire;
            _playerInputs.Character.Interact.performed -= OnInteract;
        }
    }
}
