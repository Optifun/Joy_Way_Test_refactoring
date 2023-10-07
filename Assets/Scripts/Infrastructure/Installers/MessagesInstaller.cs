using JoyWay.Core.Messages;
using MessagePipe;
using Zenject;
namespace JoyWay.Infrastructure.Installers
{
    public class MessagesInstaller : Installer
    {
        public override void InstallBindings()
        {
            var options = Container.BindMessagePipe();
            Container.BindMessageBroker<NetworkPlayerSpawnedMessage>(options);
            Container.BindMessageBroker<SpawnPlayerServerMessage>(options);
            Container.BindMessageBroker<HealthUpdateMessage>(options);
            Container.BindMessageBroker<DeathMessage>(options);
            Container.BindMessageBroker<ClientConnected>(options);
            Container.BindMessageBroker<ClientDisconnected>(options);
            Container.BindMessageBroker<ClientError>(options);
            Container.BindMessageBroker<ServerClientConnected>(options);
            Container.BindMessageBroker<ServerClientDisconnected>(options);
            Container.BindMessageBroker<ServerError>(options);
            Container.BindMessageBroker<GameEvent>(options);
        }
    }
}
