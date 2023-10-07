using System.Net;
namespace JoyWay.Core.Requests
{
    public class JoinGameRequest
    {
        public readonly IPAddress IPAddress;
        public JoinGameRequest(IPAddress ipAddress)
        {
            IPAddress = ipAddress;
        }
    }
}
