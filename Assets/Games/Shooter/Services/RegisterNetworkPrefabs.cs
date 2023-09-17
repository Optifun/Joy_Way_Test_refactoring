using System;
using System.Collections.Generic;
using JoyWay.Core.Infrastructure.AssetManagement;
using Mirror;
using UnityEngine;
using Zenject;
namespace JoyWay.Games.Shooter.Services
{
    public class RegisterNetworkPrefabs : IInitializable, IDisposable
    {
        private readonly IAssets _assets;
        private IList<GameObject> _networkPrefabs;

        public RegisterNetworkPrefabs(IAssets assets)
        {
            _assets = assets;
        }

        public async void Initialize()
        {
            _networkPrefabs = await _assets.LoadMultiple<GameObject>(new List<string>(){"networkPrefab", "shooterGame"}, false);
            foreach (var prefab in _networkPrefabs)
            {
                var networkIdentity = prefab.GetComponent<NetworkIdentity>();
                if (false == NetworkClient.GetPrefab(networkIdentity.assetId, out var _))
                {
                    NetworkClient.RegisterPrefab(prefab);
                }
            }
        }
        public void Dispose()
        {
            foreach (var prefab in _networkPrefabs)
            {
                NetworkClient.UnregisterPrefab(prefab);
            }
        }
    }
}
