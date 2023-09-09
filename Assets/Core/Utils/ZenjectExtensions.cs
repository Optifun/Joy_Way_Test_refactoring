using Zenject;
namespace JoyWay.Core.Utils
{
    public static class ZenjectExtensions
    {
        public static ConcreteIdBinderNonGeneric Bind<TContract1, TContract2>(this DiContainer container)
        {
            return container.Bind(typeof(TContract1), typeof(TContract2));
        }

        public static ConcreteIdBinderNonGeneric Bind<TContract1, TContract2, TContract3>(this DiContainer container)
        {
            return container.Bind(typeof(TContract1), typeof(TContract2), typeof(TContract3));
        }

        public static ConcreteIdBinderNonGeneric Bind<TContract1, TContract2, TContract3, TContract4>(this DiContainer container)
        {
            return container.Bind(typeof(TContract1), typeof(TContract2), typeof(TContract3), typeof(TContract4));
        }
    }
}
