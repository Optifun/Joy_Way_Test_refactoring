using JoyWay.Game;
using JoyWay.Infrastructure.Factories;
using JoyWay.Services;
using UnityEngine;
using Zenject;

namespace JoyWay.Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        [SerializeField]
        private LevelSpawnPoints _levelSpawnPoints;

        public override void InstallBindings()
        {
            Container.Bind<LevelSpawnPoints>().FromInstance(_levelSpawnPoints).AsSingle();
            Container.Bind<CharacterFactory>().FromNew().AsSingle();
            Container.Bind<ProjectileFactory>().FromNew().AsSingle();
            Container.Bind<ServerPlayerSpawnerService>().FromNew().AsSingle().NonLazy();
            Container.Bind<ClientPlayerSpawnerService>().FromMethod(context =>
            {
                var assetContainer = context.Container.Resolve<AssetContainer>();
                var characterFactory = context.Container.Resolve<CharacterFactory>();

                var characterPrefab = assetContainer.Character.Value.gameObject;
                return new ClientPlayerSpawnerService(characterPrefab, characterFactory);
            }).AsSingle().NonLazy();

            
        }
    }
}
