using JoyWay.Core.Infrastructure.AssetManagement;
using Mirror;
using UnityEngine;
using Zenject;

namespace JoyWay.Games.Shooter.Projectiles
{
    public class ProjectileFactory : IInitializable
    {
        private readonly IAssets _assets;
        private Projectile _fireBallPrefab;

        public ProjectileFactory(IAssets assets)
        {
            _assets = assets;
        }

        public async void Initialize()
        {
            var loadedPrefab = await _assets.Load<GameObject>(ShooterResources.Fireball);
            _fireBallPrefab = loadedPrefab.GetComponent<Projectile>();
        }

        public Projectile CreateFireball(Vector3 at, Vector3 direction, uint sender)
        {
            var fireball = Object.Instantiate(_fireBallPrefab, at, Quaternion.identity);
            NetworkServer.Spawn(fireball.gameObject);
            fireball.Throw(direction, sender);
            return fireball;
        }
    }
}
