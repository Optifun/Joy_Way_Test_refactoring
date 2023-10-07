using JoyWay.Core.Messages;
using JoyWay.Core.Model;
using MessagePipe;
using Zenject;

namespace JoyWay.Infrastructure
{
    public class GameStartup : IInitializable
    {
        private readonly IPublisher<GameEvent> _publisher;

        public GameStartup(IPublisher<GameEvent> publisher)
        {
            _publisher = publisher;
        }

        public void Initialize()
        {
            _publisher.Publish(new GameEvent(this, GameEventType.ServicesInitialized));
        }
    }
}