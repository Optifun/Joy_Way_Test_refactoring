using JoyWay.Games.Shooter.Services;
using UnityEngine;
using Zenject;

namespace JoyWay.Games.Shooter.Installers
{
    public class FPSCameraInstaller : MonoInstaller
    {
        [SerializeField] private FPSCameraService _fpsCameraService;

        public override void InstallBindings()
        {
            Container.Bind<FPSCameraService>()
                  .FromComponentInNewPrefab(_fpsCameraService)
                  .AsSingle()
                  .NonLazy();
        }
    }
}
