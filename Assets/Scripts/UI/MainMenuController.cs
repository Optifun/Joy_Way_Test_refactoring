using System.Net;
using JoyWay.Infrastructure;

namespace JoyWay.UI
{
    public class MainMenuController
    {
        private MainMenuUI _mainMenuUI;
        private GameFlow _gameFlow;

        public void Setup(MainMenuUI mainMenu, GameFlow gameFlow)
        {
            _gameFlow = gameFlow;
            _mainMenuUI = mainMenu;

            _mainMenuUI.HostButtonClicked.AddListener(() => _gameFlow.StartHost());
            _mainMenuUI.ConnectButtonClicked.AddListener(() => _gameFlow.StartClient(GetAddress()));
        }

        public void Show()
        {
            _mainMenuUI.Show();
        }

        public void Hide()
        {
            _mainMenuUI.Hide();
        }

        private IPAddress GetAddress()
        {
            var ipString = _mainMenuUI.GetAddress();

            if (IPAddress.TryParse(ipString, out var ipAddress))
            {
                return ipAddress;
            }
            return IPAddress.Loopback;
        }
    }
}
