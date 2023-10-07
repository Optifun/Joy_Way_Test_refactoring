using JoyWay.Core.Components;
using JoyWay.Services;
using JoyWay.UI;
using UnityEngine;
using Zenject;
namespace JoyWay.Infrastructure.Factories
{
    public class UIFactory
    {
        private readonly UIAssetContainer _assetContainer;
        private readonly AdvancedNetworkManager _networkManager;
        private readonly DiContainer _diContainer;

        public UIFactory(DiContainer diContainer, UIAssetContainer assetContainer)
        {
            _diContainer = diContainer;
            _assetContainer = assetContainer;
        }
    
        public MainMenuController CreateMainMenu()
        {
            MainMenuUI mainMenu = Object.Instantiate(_assetContainer.MainMenuUI.Value);
            Object.DontDestroyOnLoad(mainMenu);
            var controller = _diContainer.Resolve<MainMenuController>();
            controller.Setup(mainMenu);
            return controller;
        }

        public HideableUI CreateCrosshairUI()
        {
            HideableUI crosshairUI = Object.Instantiate(_assetContainer.CrosshairUI.Value);
            Object.DontDestroyOnLoad(crosshairUI);
            return crosshairUI;
        }
    }
}