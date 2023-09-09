using JoyWay.Games.Shooter.Projectiles;
using Mirror;
using UnityEngine;
using Zenject;
namespace JoyWay.Games.Shooter.Character
{
    public class NetworkCharacterShootingComponent : NetworkBehaviour
    {
        [SerializeField] private Transform _handEndTransform;
        [SerializeField] private NetworkCharacterLookComponent _lookComponent;

        private Vector3 _lookDirection;
        private ProjectileFactory _projectileFactory;

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
