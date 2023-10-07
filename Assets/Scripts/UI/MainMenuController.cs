using System.Net;
using Cysharp.Threading.Tasks;
using JetBrains.Lifetimes;
using JoyWay.Core.Handlers;
using JoyWay.Core.Requests;

namespace JoyWay.UI
{
    public class MainMenuController
    {
        private MainMenuUI _mainMenuUI;
        private readonly IHostGameRequestHandler _hostGameHandler;
        private readonly IJoinGameRequestHandler _joinGameHandler;
        private readonly Lifetime _lifetime;

        public MainMenuController(IHostGameRequestHandler hostGameHandler, IJoinGameRequestHandler joinGameHandler, Lifetime lifetime)
        {
            _lifetime = lifetime;
            _hostGameHandler = hostGameHandler;
            _joinGameHandler = joinGameHandler;
        }

        public void Setup(MainMenuUI mainMenu)
        {
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
            var ipAddress = GetAddress();
            // TODO: obtain result & exception
            _joinGameHandler.InvokeAsync(new JoinGameRequest(ipAddress), _lifetime).Forget();
        }

        private void OnStartHostClicked()
        {
            // TODO: obtain result & exception
            _hostGameHandler.InvokeAsync(new HostGameRequest(), _lifetime).Forget();
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
