using Core.Services;
using JoyWay.UI;
using Zenject;
namespace JoyWay.Infrastructure.Installers
{
    public class UIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<UIAssetContainer>().FromNew().AsSingle();
            Container.Bind<UIFactory>().FromNew().AsSingle();
            Container.Bind<MainMenuController>().FromNew().AsSingle();
        }
    }
}
