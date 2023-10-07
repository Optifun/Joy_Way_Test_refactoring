using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using JetBrains.Lifetimes;
using JoyWay.Core.Infrastructure;
using JoyWay.Core.Infrastructure.AssetManagement;
using JoyWay.Core.Messages;
using JoyWay.Core.Model;
using JoyWay.Core.Resources;
using JoyWay.Core.Services;
using JoyWay.Games.Shooter;
using JoyWay.Infrastructure.Factories;
using JoyWay.UI;
using MessagePipe;
using Microsoft.Extensions.Logging;
using Stateless;
using Zenject;
using ZLogger;
namespace JoyWay.Infrastructure
{
    public class GameStateMachine : IInitializable
    {
        private readonly StateMachine<GameStateType, GameEventType> _stateMachine;
        private readonly GameState _gameState;
        private readonly Lifetime _lifetime;
        private readonly SceneLoader _sceneLoader;
        private readonly UIFactory _factory;
        private readonly IAssets _assets;
        private readonly InputService _inputService;
        private readonly IAsyncSubscriber<GameEvent> _asyncSubscriber;
        private readonly ILogger<GameStateMachine> _logger;
        private readonly ISubscriber<GameEvent> _subscriber;
        private IDisposable _subscription;
        private MainMenuController _mainMenuController; // TODO: remove dependency on UI
        private HideableUI _crosshairUI;

        public GameStateMachine(
            GameState gameState,
            Lifetime lifetime,
            SceneLoader sceneLoader,
            InputService inputService,
            UIFactory factory,
            IAssets assets,
            IAsyncSubscriber<GameEvent> asyncSubscriber,
            ISubscriber<GameEvent> subscriber,
            ILoggerFactory loggerFactory)
        {
            _lifetime = lifetime;
            _inputService = inputService;
            _subscriber = subscriber;
            _gameState = gameState;
            _assets = assets;
            _sceneLoader = sceneLoader;
            _factory = factory;
            _asyncSubscriber = asyncSubscriber;
            _logger = loggerFactory.CreateLogger<GameStateMachine>();

            _stateMachine = new StateMachine<GameStateType, GameEventType>(() => gameState.State, gameState.SetState);
            _stateMachine.Configure(GameStateType.Entry)
                      .OnExit(WarmupAssets)
                      .Permit(GameEventType.ServicesInitialized, GameStateType.Menu);

            _stateMachine.Configure(GameStateType.Menu)
                      .OnEntryAsync(OpenMenuScene)
                      .OnActivate(ShowMainMenu)
                      .OnExit(HideMainMenu)
                      .Permit(GameEventType.HostGame, GameStateType.HostGame)
                      .Permit(GameEventType.JoinGame, GameStateType.JoinGame);

            _stateMachine.Configure(GameStateType.HostGame)
                      .OnEntry(OnHostGame)
                      .OnActivateAsync(LoadGameScene)
                      .Permit(GameEventType.GameLoaded, GameStateType.Game);

            _stateMachine.Configure(GameStateType.JoinGame)
                      .OnEntry(OnJoinGame)
                      .OnActivateAsync(LoadGameScene)
                      .Permit(GameEventType.GameLoaded, GameStateType.Game);

            _stateMachine.Configure(GameStateType.Game)
                      .OnEntry(OnGameEnter)
                      .OnExit(OnGameExit)
                      .Permit(GameEventType.ShowResults, GameStateType.Results)
                      .Permit(GameEventType.Disconnected, GameStateType.Menu);
        }

        public void Initialize()
        {
            _lifetime.AddDispose(_asyncSubscriber.Subscribe(ProcessEvent));
            _lifetime.AddDispose(_subscriber.Subscribe(ProcessEvent));
        }

        private void WarmupAssets()
        {
            _mainMenuController = _factory.CreateMainMenu();
            _crosshairUI = _factory.CreateCrosshairUI();
        }

        private async Task OpenMenuScene(StateMachine<GameStateType, GameEventType>.Transition e)
        {
            await _sceneLoader.LoadAsync(CoreResources.MenuScene);
        }

        private void ShowMainMenu()
        {
            _mainMenuController.Show();
            _crosshairUI.Hide();
        }

        private void HideMainMenu()
        {
            _mainMenuController.Hide();
        }

        private void OnHostGame()
        {
            _gameState.IsServer = true;
            _gameState.IsClient = true;
        }

        private void OnJoinGame()
        {
            _gameState.IsServer = false;
            _gameState.IsClient = true;
        }

        private async Task LoadGameScene() // TODO: redirect to game module
        {
            await _assets.LoadScene(ShooterResources.ShooterGameScene).AsTask();
        }

        private void OnGameEnter()
        {
            _inputService.Enable();
            _crosshairUI.Show();
        }

        private void OnGameExit()
        {
            _inputService.Disable();
            _gameState.IsServer = false;
            _gameState.IsClient = false;
            _crosshairUI.Hide();
        }

        private void ProcessEvent(GameEvent e)
        {
            _logger.ZLogTrace("Raised game Event {0} by Sender {1}", e.Event, e.Sender);
            _stateMachine.FireAsync(e.Event).AsUniTask().Forget(); // TODO: wtf
        }

        private async UniTask ProcessEvent(GameEvent e, CancellationToken token)
        {
            _logger.ZLogTrace("Raised game Event {0} by Sender {1}", e.Event, e.Sender);
            await _stateMachine.FireAsync(e.Event);
        }
    }
}
