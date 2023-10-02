using JoyWay.Core.Services;
using Zenject;

namespace JoyWay.Infrastructure.Installers
{
    public class InputInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerInputs>().AsSingle();

            Container.Bind<InputService>()
                  .FromNewComponentOnNewGameObject()
                  .AsSingle();
        }
    }
}
