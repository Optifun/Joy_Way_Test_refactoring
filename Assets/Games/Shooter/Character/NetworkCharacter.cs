using System;
using JoyWay.Core.Services;
using Mirror;
using UnityEngine;
using Zenject;
namespace JoyWay.Games.Shooter.Character
{
    public class NetworkCharacter : NetworkBehaviour
    {

        [field: SerializeField]
        public CharacterContainer Container { get; private set; }
        private InputService _inputService;

        private void OnDestroy()
        {
            _inputService.Move -= Container.NetworkMovement.Move;
            _inputService.Jump -= Container.NetworkMovement.Jump;
            _inputService.Interact -= Container.NetworkInteraction.Interact;
            _inputService.Fire -= Container.NetworkShooting.Fire;
            OnDestroyed?.Invoke(this);
        }
        public event Action<NetworkCharacter> OnDestroyed;

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
    }
}
