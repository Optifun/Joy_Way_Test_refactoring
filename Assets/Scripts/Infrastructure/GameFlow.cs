using System.Net;
using Cysharp.Threading.Tasks;
using JoyWay.Resources;
using JoyWay.Services;
using JoyWay.UI;
using UnityEngine;
using Zenject;

namespace JoyWay.Infrastructure
{
    public class GameFlow : MonoBehaviour, ILaunchContext
    {
        private AdvancedNetworkManager _networkManager;
        private UIFactory _uiFactory;

        private MainMenuController _mainMenu;

        private HideableUI _crosshairUI;
        private InputService _inputService;
        private SceneLoader _sceneLoader;

        public bool IsClient { get; private set; }
        public bool IsServer { get; private set; }

        [Inject]
        public void Construct(AdvancedNetworkManager networkManager, UIFactory uiFactory, InputService inputService, SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
            _inputService = inputService;
            _networkManager = networkManager;
            _uiFactory = uiFactory;
        }

        public void StartGame()
        {
            _mainMenu = _uiFactory.CreateMainMenu(this);
            _crosshairUI = _uiFactory.CreateCrosshairUI();
            _networkManager.ClientDisconnected += ExitGame;
            GoToMenu().Forget();
        }

        public void ExitGame()
        {
            IsServer = false;
            IsClient = false;
            GoToMenu().Forget();
        }

        public async UniTask StartClientAsync(IPAddress address)
        {
            IsClient = true;
            await _sceneLoader.LoadAsync(Constants.GameScene);
            await _networkManager.ConnectAsync(address);
            GoToGame();
        }

        public async UniTask StartHostAsync()
        {
            IsServer = true;
            IsClient = true;
            await _sceneLoader.LoadAsync(Constants.GameScene);
            await _networkManager.StartHostAsync();
            GoToGame();
        }


        private async UniTask GoToMenu()
        {
            _inputService.Disable();
            await _sceneLoader.LoadAsync(Constants.MenuScene);
            _mainMenu.Show();
            _crosshairUI.Hide();
        }

        private void GoToGame()
        {
            _inputService.Enable();
            _mainMenu.Hide();
            _crosshairUI.Show();
        }

        private void OnDestroy()
        {
            _networkManager.ClientDisconnected -= ExitGame;
        }
    }
}
