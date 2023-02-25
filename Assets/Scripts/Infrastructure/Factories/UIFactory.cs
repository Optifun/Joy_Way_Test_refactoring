using JoyWay.Infrastructure;
using JoyWay.Services;
using JoyWay.UI;
using UnityEngine;

public class UIFactory
{
    private readonly AssetContainer _assetContainer;
    private readonly AdvancedNetworkManager _networkManager;

    public UIFactory(AssetContainer assetContainer, AdvancedNetworkManager networkManager)
    {
        _assetContainer = assetContainer;
        _networkManager = networkManager;
    }
    
    public MainMenuController CreateMainMenu()
    {
        MainMenuUI mainMenu = Object.Instantiate(_assetContainer.MainMenuUI.Value);
        Object.DontDestroyOnLoad(mainMenu);
        return new MainMenuController(mainMenu, _networkManager);
    }

    public HideableUI CreateCrosshairUI()
    {
        HideableUI crosshairUI = Object.Instantiate(_assetContainer.CrosshairUI.Value);
        Object.DontDestroyOnLoad(crosshairUI);
        return crosshairUI;
    }
}