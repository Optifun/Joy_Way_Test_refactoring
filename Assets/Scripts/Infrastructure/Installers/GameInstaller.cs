using JoyWay.Game;
using JoyWay.Game.Character;
using JoyWay.Game.Services;
using JoyWay.Infrastructure.Factories;
using JoyWay.Services;
using JoyWay.Utils;
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
            var launchContext = Container.Resolve<ILaunchContext>();
            Container.Bind<LevelSpawnPoints>().FromInstance(_levelSpawnPoints).AsSingle();
            Container.Bind<CharacterFactory>().FromNew().AsSingle();
            Container.Bind<ProjectileFactory>().FromNew().AsSingle();
            Container.Bind<IInitializable, CharacterDeathSystem>()
                .To<CharacterDeathSystem>().FromNew().AsSingle();
            Container.Bind<DamageController>().FromNew().AsTransient();

            Container.Bind<IInitializable, ServerPlayerSpawnerSystem>()
                .To<ServerPlayerSpawnerSystem>().FromNew().AsSingle().When(IsServer);

            Container.Bind<IInitializable, ServerRespawnPlayerService>()
                .To<ServerRespawnPlayerService>().FromNew().AsSingle().When(IsServer);

            Container.Bind<GameObject>().FromMethod(context => context.Container.Resolve<AssetContainer>().Character.Value.gameObject)
                .AsTransient().WhenInjectedInto<ClientPlayerSpawnerSystem>();
            Container.Bind<IInitializable, ClientPlayerSpawnerSystem>().To<ClientPlayerSpawnerSystem>().FromNew().AsSingle().When(IsClient);

        }

        private static bool IsServer(InjectContext context)
        {
            return context.Container.Resolve<ILaunchContext>().IsServer;
        }

        private static bool IsClient(InjectContext context)
        {
            return context.Container.Resolve<ILaunchContext>().IsClient;
        }
    }
}
