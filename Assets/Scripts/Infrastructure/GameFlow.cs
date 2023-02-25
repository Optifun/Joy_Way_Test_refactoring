using JoyWay.Resources;
using JoyWay.Services;
using JoyWay.UI;
using UnityEngine;
using Zenject;

namespace JoyWay.Infrastructure
{
    public class GameFlow : MonoBehaviour
    {
        private AdvancedNetworkManager _networkManager;
        private UIFactory _uiFactory;

        private MainMenuController _mainMenu;

        private HideableUI _crosshairUI;
        private InputService _inputService;
        private SceneLoader _sceneLoader;

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
            _mainMenu = _uiFactory.CreateMainMenu();
            _crosshairUI = _uiFactory.CreateCrosshairUI();
            _networkManager.Connected += GoToGame;
            _networkManager.Disconnected += GoToMenu;
            GoToMenu();
        }

        private void GoToMenu()
        {
            _sceneLoader.Load(Constants.MenuScene);
            _mainMenu.Show();
            _crosshairUI.Hide();
            _inputService.Disable();
        }

        private void GoToGame()
        {
            _sceneLoader.Load(Constants.GameScene);
            _mainMenu.Hide();
            _crosshairUI.Show();
            _inputService.Enable();
        }

        private void OnDestroy()
        {
            _networkManager.Connected -= GoToGame;
            _networkManager.Disconnected -= GoToMenu;
        }
    }
}