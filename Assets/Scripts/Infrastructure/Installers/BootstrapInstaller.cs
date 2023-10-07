using JoyWay.Core.Components;
using JoyWay.Core.Handlers;
using JoyWay.Core.Infrastructure;
using JoyWay.Core.Infrastructure.AssetManagement;
using JoyWay.Core.Services;
using JoyWay.Core.Utils;
using UnityEngine;
using Zenject;

namespace JoyWay.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller<BootstrapInstaller>
    {
        [SerializeField] private AdvancedNetworkManager _networkManagerPrefab;
        
        public override void InstallBindings()
        {
            Container.Install<InputInstaller>();
            Container.Install<MessagesInstaller>();
            Container.Install<LoggingInstaller>();

            Container.Bind<PrefabSpawner>().ToSelf().AsTransient().CopyIntoAllSubContainers();
            Container.BindInterfacesTo<AssetProvider>().FromNew().AsCached();
            Container.Bind<SceneLoader>().ToSelf().AsSingle();

            Container.Bind<AdvancedNetworkManager, ICoroutineRunner>()
                  .FromComponentInNewPrefab(_networkManagerPrefab)
                  .AsSingle()
                  .NonLazy();

            Container.BindInterfacesAndSelfTo<NetworkManagerHandler>().AsSingle();

            Container.BindInterfacesAndSelfTo<GameState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();

            Container.Bind<IInitializable>()
                .To<GameStartup>()
                .AsSingle()
                .NonLazy();

            // Container.Bind<GameFlow>().ToSelf()
            //     .FromNewComponentOnNewGameObject()
            //     .AsSingle()
            //     .NonLazy();

        }
    }
}