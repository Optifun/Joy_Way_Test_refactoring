using JoyWay.Infrastructure.Factories;
using Zenject;

namespace JoyWay.Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller<BootstrapInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<LaunchParameters>().FromMethod(context =>
            {
                var networkManager = context.Container.Resolve<AdvancedNetworkManager>();
                return new LaunchParameters() { IsClient = networkManager.IsClient, IsServer = networkManager.IsServer };
            });

            Container.Bind<CharacterFactory>().FromNew().AsSingle();
            Container.Bind<ProjectileFactory>().FromNew().AsSingle();
        }
    }
}
