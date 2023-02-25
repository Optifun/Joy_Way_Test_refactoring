using JoyWay.Game.Messages;
using JoyWay.Game.Services;
using JoyWay.Services;
using JoyWay.UI;
using JoyWay.Utils;
using MessagePipe;
using UnityEngine;
using Zenject;

namespace JoyWay.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller<BootstrapInstaller>
    {
        [SerializeField] private CameraService _cameraService;
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

            Container.Bind<UIFactory>().FromNew().AsSingle().NonLazy();
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
            
            Container.Bind<SceneLoader>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .NonLazy();

            Container.Bind<CameraService>()
                .FromComponentInNewPrefab(_cameraService)
                .AsSingle()
                .NonLazy();
        }
    }
}