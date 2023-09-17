using System;
using JoyWay.Core.Components;
using JoyWay.Core.Messages;
using JoyWay.Core.Services;
using JoyWay.Core.Utils;
using JoyWay.Games.Shooter.Character;
using JoyWay.Games.Shooter.Projectiles;
using JoyWay.Games.Shooter.Services;
using JoyWay.Games.Shooter.StaticData;
using MessagePipe;
using UnityEngine;
using Zenject;

namespace JoyWay.Games.Shooter.Installers
{
    public class GameInstaller : MonoInstaller<GameInstaller>, IInitializable
    {
        [SerializeField] private LevelSpawnPoints _levelSpawnPoints;
        [SerializeField] private CharacterConfig _characterConfig;
        [Inject] private ILaunchContext _launchContext;
        [Inject] private IBufferedSubscriber<NetworkPlayerSpawnedMessage> _serverClientConnected;
        [Inject] private IBufferedSubscriber<ServerClientDisconnected> _serverClientDisconnected;
        [Inject] private IBufferedSubscriber<ClientConnected> _clientConnected;
        [Inject] private IBufferedSubscriber<ClientDisconnected> _clientDisconnected;

        public void Initialize() // TODO: move to fps game installer
        {
            var cameraService = Container.Resolve<FPSCameraService>();

            _clientConnected.Subscribe(_ =>
            {
                cameraService.SetFpsCamera(true);
            });
            _clientDisconnected.Subscribe(_ =>
            {
                cameraService.SetFpsCamera(false);
            });
            // NetworkPlayersCollection playersCollection = Container.Resolve<NetworkPlayersCollection>();
            // _serverClientConnected.Subscribe(spawned =>
            // {
            //     playersCollection.AddPlayer(spawned.Player);
            // });
            // _serverClientDisconnected.Subscribe(disconnected =>
            // {
            //     playersCollection.RemovePlayer(disconnected.Connection);
            // });
        }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<NetworkPlayerCollection>()
                  .FromComponentsInHierarchy().AsSingle()
                  .When(IsServer).NonLazy();

            Container.Bind<IInitializable>().FromInstance(this).AsSingle();
            Container.Bind<LevelSpawnPoints>().FromInstance(_levelSpawnPoints).AsSingle();
            Container.Bind<CharacterConfig>().FromInstance(_characterConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<RegisterNetworkPrefabs>().AsSingle();
            Container.Bind<CharacterFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<ProjectileFactory>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<CharacterDeathSystem>().AsSingle();
            Container.Bind<DamageController>().FromNew().AsTransient();

            Container.BindInterfacesAndSelfTo<ServerPlayerSpawnerSystem>().AsSingle().When(IsServer);
            Container.BindInterfacesAndSelfTo<ServerRespawnPlayerService>().AsSingle().When(IsServer);


            Container.BindInterfacesAndSelfTo<ClientPlayerSpawnerSystem>().AsSingle().When(IsClient);
        }

        private bool IsServer(InjectContext _)
        {
            return _launchContext.IsServer;
        }

        private bool IsClient(InjectContext _)
        {
            return _launchContext.IsClient;
        }
    }
}
