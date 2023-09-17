using JoyWay.Core.Components;
using Mirror;

namespace JoyWay.Core.Messages
{
    public class ClientConnected
    {
        public readonly bool IsLocal;
        public ClientConnected(bool isLocal)
        {
            IsLocal = isLocal;
        }
    }

    public class ClientDisconnected
    {
        public readonly bool IsLocal;
        public ClientDisconnected(bool isLocal)
        {
            IsLocal = isLocal;
        }
    }

    public class ClientError
    {
        private TransportError _error;
        private string _reason;

        public ClientError(TransportError error, string reason)
        {
            _reason = reason;
            _error = error;
        }
    }

    public class ServerClientConnected
    {
        public readonly NetworkConnectionToClient Connection;
        public ServerClientConnected(NetworkConnectionToClient connection)
        {
            Connection = connection;

        }
    }

    public class ServerClientDisconnected
    {
        public readonly NetworkConnectionToClient Connection;

        public ServerClientDisconnected(NetworkConnectionToClient connection)
        {
            Connection = connection;
        }
    }

    public class ServerError
    {
        public readonly NetworkConnectionToClient Connection;
        public readonly TransportError Error;
        public readonly string Reason;

        public ServerError(NetworkConnectionToClient connection, TransportError error, string reason)
        {
            Reason = reason;
            Error = error;
            Connection = connection;
        }
    }

    public class SpawnPlayerServerMessage
    {
        public readonly NetworkConnectionToClient Connection;

        public SpawnPlayerServerMessage(NetworkConnectionToClient connection)
        {
            Connection = connection;
        }
    }

    public class NetworkPlayerSpawnedMessage
    {
        public readonly NetworkConnectionToClient Connection;
        public readonly NetworkPlayer Player;

        public NetworkPlayerSpawnedMessage(NetworkConnectionToClient connection, NetworkPlayer player)
        {
            Connection = connection;
            Player = player;
        }
    }
}
