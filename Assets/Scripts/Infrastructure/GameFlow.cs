using System.Collections;
using System.Net;
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
            _networkManager.Disconnected += ExitGame;
            GoToMenu();
        }

        public void ExitGame()
        {
            IsServer = false;
            IsClient = false;
            GoToMenu();
        }

        public void StartClient(IPAddress address)
        {
            IsClient = true;
            StartCoroutine(StartClientRoutine(address));
        }

        public void StartHost()
        {
            IsServer = true;
            IsClient = true;
            StartCoroutine(StartHostRoutine());
        }


        private IEnumerator StartClientRoutine(IPAddress ipAddress)
        {
            yield return _sceneLoader.Load(Constants.GameScene);
            _networkManager.Connect(ipAddress);
            GoToGame();
        }

        private IEnumerator StartHostRoutine()
        {
            yield return _sceneLoader.Load(Constants.GameScene);
            _networkManager.StartHost();
            GoToGame();
        }

        private void GoToMenu()
        {
            _inputService.Disable();
            _sceneLoader.Load(Constants.MenuScene);
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
            _networkManager.Disconnected -= ExitGame;
        }
    }
}
