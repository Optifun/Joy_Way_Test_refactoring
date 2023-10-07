using JetBrains.Lifetimes;
using Zenject;

namespace JoyWay.Infrastructure.Installers
{
    /// <summary>
    /// Creates Lifetime for each container
    /// </summary>
    public class LifeTimeInstaller : MonoInstaller
    {
        [Inject(Optional = true)] private LifetimeDefinition _parentLifeTime;

        public override void InstallBindings()
        {
            Container.Bind<LifetimeDefinition>()
                  .FromMethod(_ =>
                      {
                          Lifetime parentLifeTime = _parentLifeTime?.Lifetime ?? Lifetime.Eternal;
                          return Lifetime.Define(parentLifeTime, name);
                      })
                  .AsCached();

            Container.Bind<Lifetime>()
                  .FromMethod(context => context.Container.Resolve<LifetimeDefinition>().Lifetime)
                  .AsTransient();
        }
    }
}
