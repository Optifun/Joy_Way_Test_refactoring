using Core.Services;
using JoyWay.Game.Projectiles;
using JoyWay.Services;
using Mirror;
using UnityEngine;

namespace JoyWay.Infrastructure.Factories
{
    public class ProjectileFactory
    {
        private AssetContainer _assetContainer;
        
        public ProjectileFactory(AssetContainer assetContainer)
        {
            _assetContainer = assetContainer;
        }
        
        public Projectile CreateFireball(Vector3 at, Vector3 direction, uint sender)
        {
            Projectile fireball = Object.Instantiate(_assetContainer.Fireball.Value, at, Quaternion.identity);
            NetworkServer.Spawn(fireball.gameObject);
            fireball.Throw(direction, sender);
            return fireball;
        }
    }
}