using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace JoyWay.Core.Components
{
    public class NetworkPlayerCollection : NetworkBehaviour, IPlayersCollection
    {
        public IReadOnlyList<NetworkPlayer> Players => _players;
        public IReadOnlyDictionary<NetworkConnectionToClient, NetworkPlayer> PlayerDictionary => _playerDictionary;

        private readonly SyncList<NetworkPlayer> _players = new SyncList<NetworkPlayer>();
        private Dictionary<NetworkConnectionToClient, NetworkPlayer> _playerDictionary = new();

        [Server]
        public void AddPlayer(NetworkPlayer player)
        {
            _players.Add(player);
            _playerDictionary.Add(player.connectionToClient, player);
        }

        [Server]
        public void RemovePlayer(NetworkPlayer player)
        {
            _players.Remove(player);
            _playerDictionary.Remove(player.connectionToClient);
        }

        [Server]
        public void RemovePlayer(NetworkConnectionToClient conn)
        {
            RemovePlayer(_playerDictionary[conn]);
        }

        [ContextMenu(nameof(PrintPlayers))]
        private void PrintPlayers()
        {
            foreach (NetworkPlayer networkPlayer in Players)
            {
                Debug.Log(networkPlayer.netId);
            }
        }
    }

    public interface IPlayersCollection
    {
        IReadOnlyList<NetworkPlayer> Players { get; }
    }
}
