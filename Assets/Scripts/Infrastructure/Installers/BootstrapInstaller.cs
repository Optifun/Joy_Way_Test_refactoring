using JoyWay.Services;
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

            Container.Bind<AdvancedNetworkManager>()
                .FromComponentInNewPrefab(_networkManagerPrefab)
                .AsSingle()
                .NonLazy();

            Container.Bind<IInitializable>()
                .To<GameStartup>()
                .AsSingle()
                .NonLazy();

            Container.Bind<GameFlow>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .NonLazy();
        }

        private void InstallServices()
        {
            Container.Bind<AssetContainer>()
                .FromNew()
                .AsSingle()
                .NonLazy();
            
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