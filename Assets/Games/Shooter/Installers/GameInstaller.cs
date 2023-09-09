using JoyWay.Core.Components;
using JoyWay.Core.Services;
using JoyWay.Core.Utils;
using JoyWay.Games.Shooter.Character;
using JoyWay.Games.Shooter.Projectiles;
using JoyWay.Games.Shooter.Services;
using UnityEngine;
using Zenject;
namespace JoyWay.Games.Shooter.Installers
{
    public class GameInstaller : MonoInstaller<GameInstaller>, IInitializable
    {
        [SerializeField]
        private LevelSpawnPoints _levelSpawnPoints;

        public void Initialize() // TODO: move to fps game installer
        {
            var networkManager = Container.Resolve<AdvancedNetworkManager>();
            var cameraService = Container.Resolve<FPSCameraService>();
            networkManager.ClientConnected += () => cameraService.SetFpsCamera(true);
            networkManager.ClientDisconnected += () => cameraService.SetFpsCamera(false);
        }

        public override void InstallBindings()
        {
            Container.Bind<IInitializable>().FromInstance(this).AsSingle();
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
