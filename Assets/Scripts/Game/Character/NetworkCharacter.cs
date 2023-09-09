using System;
using JoyWay.Services;
using Mirror;
using UnityEngine;
using Zenject;

namespace JoyWay.Game.Character
{
    public class NetworkCharacter : NetworkBehaviour
    {
        public event Action<NetworkCharacter> OnDestroyed;

        [field: SerializeField]
        public CharacterContainer Container { get; private set; }
        private InputService _inputService;

        [Inject]
        private void Initialize(InputService inputService)
        {
            _inputService = inputService;
        }

        public void SetupLocalPlayer()
        {
            _inputService.Move += Container.NetworkMovement.Move;
            _inputService.Jump += Container.NetworkMovement.Jump;
            _inputService.Interact += Container.NetworkInteraction.Interact;
            _inputService.Fire += Container.NetworkShooting.Fire;
        }

        private void OnDestroy()
        {
            _inputService.Move -= Container.NetworkMovement.Move;
            _inputService.Jump -= Container.NetworkMovement.Jump;
            _inputService.Interact -= Container.NetworkInteraction.Interact;
            _inputService.Fire -= Container.NetworkShooting.Fire;
            OnDestroyed?.Invoke(this);
        }
    }
}