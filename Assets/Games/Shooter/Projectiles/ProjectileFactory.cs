using JoyWay.Games.Shooter.Services;
using Mirror;
using UnityEngine;
namespace JoyWay.Games.Shooter.Projectiles
{
    public class ProjectileFactory
    {
        private readonly AssetContainer _assetContainer;

        public ProjectileFactory(AssetContainer assetContainer)
        {
            _assetContainer = assetContainer;
        }

        public Projectile CreateFireball(Vector3 at, Vector3 direction, uint sender)
        {
            var fireball = Object.Instantiate(_assetContainer.Fireball.Value, at, Quaternion.identity);
            NetworkServer.Spawn(fireball.gameObject);
            fireball.Throw(direction, sender);
            return fireball;
        }
    }
}
