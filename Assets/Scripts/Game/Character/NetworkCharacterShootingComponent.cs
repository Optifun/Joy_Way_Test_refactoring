using JoyWay.Infrastructure.Factories;
using JoyWay.Services;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace JoyWay.Game.Character
{
    public class NetworkCharacterShootingComponent : NetworkBehaviour
    {
        [SerializeField] private Transform _handEndTransform;
        [SerializeField] private NetworkCharacterLookComponent _lookComponent;
        private ProjectileFactory _projectileFactory;

        private Vector3 _lookDirection;

        [Inject]
        public void Initialize(ProjectileFactory projectileFactory)
        {
            _projectileFactory = projectileFactory;
        }

        public void Fire()
        {
            _lookDirection = _lookComponent.LookDirection;
            CmdFire(_handEndTransform.position, _lookDirection);
        }

        [Command]
        private void CmdFire(Vector3 position, Vector3 lookDirection)
        {
            _projectileFactory.CreateFireball(position, lookDirection, netIdentity.netId);
        }
    }
}