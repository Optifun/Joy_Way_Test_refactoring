using Microsoft.Extensions.Logging;
using Zenject;
using ZLogger;
namespace JoyWay.Infrastructure.Installers
{
    public class LoggingInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ILoggerFactory>().FromMethod(CreateLoggerFactory);
            Container.Bind<ILogger>().FromMethod(CreateGlobalLogger);
        }

        private ILogger CreateGlobalLogger(InjectContext context)
        {
            return context.Container.Resolve<ILoggerFactory>()
                       .CreateLogger("Global");
        }

        private static ILoggerFactory CreateLoggerFactory(InjectContext context)
        {

            // Standard LoggerFactory does not work on IL2CPP,
            // But you can use ZLogger's UnityLoggerFactory instead,
            // it works on IL2CPP, all platforms(includes mobile).
            return UnityLoggerFactory.Create(builder =>
            {
                // or more configuration, you can use builder.AddFilter
                builder.SetMinimumLevel(LogLevel.Trace);

                // AddZLoggerUnityDebug is only available for Unity, it send log to UnityEngine.Debug.Log.
                // LogLevels are translate to
                // * Trace/Debug/Information -> LogType.Log
                // * Warning/Critical -> LogType.Warning
                // * Error without Exception -> LogType.Error
                // * Error with Exception -> LogException
                builder.AddZLoggerUnityDebug();

                // and other configuration(AddFileLog, etc...)
            });
        }
    }
}
