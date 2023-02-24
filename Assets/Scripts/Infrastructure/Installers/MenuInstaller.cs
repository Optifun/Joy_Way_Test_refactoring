using Zenject;

namespace JoyWay.Infrastructure.Installers
{
	public class MenuInstaller : MonoInstaller<BootstrapInstaller>
	{
		public override void InstallBindings()
		{
			Container.Bind<UIFactory>().FromNew().AsSingle().NonLazy();
		}
	}
}