using Core.Services;
using JoyWay.Core.Components;
using JoyWay.Core.Messages;
using JoyWay.Core.Services;
using JoyWay.Core.Utils;
using JoyWay.Games.Shooter.Services;
using JoyWay.UI;
using MessagePipe;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace JoyWay.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller<BootstrapInstaller>
    {
        [FormerlySerializedAs("_cameraService")][SerializeField] private FPSCameraService _fpsCameraService;
        [SerializeField] private AdvancedNetworkManager _networkManagerPrefab;
        
        public override void InstallBindings()
        {
            InstallServices();

            Container.Bind<AdvancedNetworkManager, ICoroutineRunner>()
                .FromComponentInNewPrefab(_networkManagerPrefab)
                .AsSingle()
                .NonLazy();

            Container.Bind<IInitializable>()
                .To<GameStartup>()
                .AsSingle()
                .NonLazy();

            Container.Bind<ILaunchContext, GameFlow>().To<GameFlow>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .NonLazy();

            Container.Bind<UIAssetContainer>().FromNew().AsSingle();
            Container.Bind<UIFactory>().FromNew().AsSingle();
            Container.Bind<MainMenuController>().FromNew().AsSingle();

            var options = Container.BindMessagePipe();
            Container.BindMessageBroker<SpawnCharacterServerMessage>(options);
            Container.BindMessageBroker<HealthUpdateMessage>(options);
            Container.BindMessageBroker<DeathMessage>(options);
        }

        private void InstallServices()
        {
            Container.Bind<AssetContainer>()
                .FromNew()
                .AsSingle()
                .NonLazy();

            Container.Bind<PlayerInputs>()
                .FromNew()
                .AsSingle();
            
            Container.Bind<InputService>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .NonLazy();

            Container.Bind<SceneLoader>().ToSelf().AsSingle();

            Container.Bind<FPSCameraService>()
                .FromComponentInNewPrefab(_fpsCameraService)
                .AsSingle()
                .NonLazy();
        }
    }
}