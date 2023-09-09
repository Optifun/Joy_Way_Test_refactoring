using Core.Services;
using JoyWay.Core.Components;
using JoyWay.Infrastructure;
using JoyWay.UI;
using UnityEngine;
using Zenject;

public class UIFactory
{
    private readonly UIAssetContainer _assetContainer;
    private readonly AdvancedNetworkManager _networkManager;
    private readonly DiContainer _diContainer;

    public UIFactory(DiContainer diContainer, UIAssetContainer assetContainer, AdvancedNetworkManager networkManager)
    {
        _diContainer = diContainer;
        _assetContainer = assetContainer;
        _networkManager = networkManager;
    }
    
    public MainMenuController CreateMainMenu(GameFlow gameFlow)
    {
        MainMenuUI mainMenu = Object.Instantiate(_assetContainer.MainMenuUI.Value);
        Object.DontDestroyOnLoad(mainMenu);
        var controller = _diContainer.Resolve<MainMenuController>();
        controller.Setup(mainMenu, gameFlow);
        return controller;
    }

    public HideableUI CreateCrosshairUI()
    {
        HideableUI crosshairUI = Object.Instantiate(_assetContainer.CrosshairUI.Value);
        Object.DontDestroyOnLoad(crosshairUI);
        return crosshairUI;
    }
}