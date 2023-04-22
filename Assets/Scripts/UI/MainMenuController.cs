using System.Net;
using Cysharp.Threading.Tasks;
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

            _mainMenuUI.HostButtonClicked.AddListener(OnStartHostClicked);
            _mainMenuUI.ConnectButtonClicked.AddListener(OnConnectClicked);
        }

        public void Show()
        {
            _mainMenuUI.Show();
        }

        public void Hide()
        {
            _mainMenuUI.Hide();
        }

        private void OnConnectClicked()
        {
            _gameFlow.StartClientAsync(GetAddress()).Forget();
        }
        private void OnStartHostClicked()
        {
            _gameFlow.StartHostAsync().Forget();
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
