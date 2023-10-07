using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using JoyWay.Core.Components;
using JoyWay.Core.Messages;
using JoyWay.Core.Model;
using JoyWay.Core.Requests;
using MessagePipe;
using Microsoft.Extensions.Logging;
namespace JoyWay.Core.Handlers
{
    public interface IJoinGameRequestHandler : IAsyncRequestHandler<JoinGameRequest, bool>
    {
    }

    public interface IHostGameRequestHandler : IAsyncRequestHandler<HostGameRequest, bool>
    {
    }

    public class NetworkManagerHandler : IHostGameRequestHandler, IJoinGameRequestHandler // TODO: move to state machine?
    {
        private readonly AdvancedNetworkManager _networkManager;
        private readonly ILogger _logger;
        private readonly IAsyncPublisher<GameEvent> _gameEvent;

        public NetworkManagerHandler(AdvancedNetworkManager networkManager, ILogger logger, IAsyncPublisher<GameEvent> gameEvent)
        {
            _logger = logger;
            _networkManager = networkManager;
            _gameEvent = gameEvent;
        }

        public async UniTask<bool> InvokeAsync(HostGameRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                await _gameEvent.PublishAsync(new GameEvent(this, GameEventType.HostGame), cancellationToken);
                await _networkManager.StartHostAsync();
                await _gameEvent.PublishAsync(new GameEvent(this, GameEventType.GameLoaded), cancellationToken);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not create host");
                return false;
            }
        }

        public async UniTask<bool> InvokeAsync(JoinGameRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                await _gameEvent.PublishAsync(new GameEvent(this, GameEventType.JoinGame), cancellationToken);
                await _networkManager.ConnectAsync(request.IPAddress);
                await _gameEvent.PublishAsync(new GameEvent(this, GameEventType.GameLoaded), cancellationToken);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not connect to server");
                return false;
            }
        }
    }
}
