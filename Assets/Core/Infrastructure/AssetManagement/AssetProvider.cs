using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace JoyWay.Core.Infrastructure.AssetManagement
{
    public class AssetProvider : IAssets, IInitializable
    {
        private readonly Dictionary<string, AddressableData> _addressableData = new Dictionary<string, AddressableData>();
        private PrefabSpawner _prefabSpawner;

        public AssetProvider(PrefabSpawner prefabSpawner)
        {
            _prefabSpawner = prefabSpawner;
        }

        public void Initialize()
        {
            Addressables.InitializeAsync();
        }

        public async UniTask<T> Load<T>(AssetReference assetReference) where T : class
        {
            if (_addressableData.TryGetValue(assetReference.AssetGUID, out AddressableData handle) && handle.Ready)
                return GetHandlesResult<T>(handle);

            return await RunWithCacheOnComplete(
                Addressables.LoadAssetAsync<T>(assetReference),
                cacheKey: assetReference.AssetGUID);
        }

        public async UniTask<T> Load<T>(string address) where T : class
        {
            if (_addressableData.TryGetValue(address, out AddressableData handle) && handle.Ready)
                return GetHandlesResult<T>(handle);

            return await RunWithCacheOnComplete(
                Addressables.LoadAssetAsync<T>(address),
                cacheKey: address);
        }

        public async UniTask<GameObject> Instantiate(string address)
        {
            var prefab = await Load<GameObject>(address);
            return _prefabSpawner.Spawn(prefab);
        }

        public async UniTask<GameObject> Instantiate(string address, Vector3 at, Quaternion rotation)
        {
            var prefab = await Load<GameObject>(address);
            return _prefabSpawner.Spawn(prefab, at, rotation);
        }

        public async UniTask<GameObject> Instantiate(AssetReference assetReference)
        {
            var prefab = await Load<GameObject>(assetReference);
            return _prefabSpawner.Spawn(prefab);
        }

        public async UniTask<GameObject> Instantiate(AssetReference assetReference, Vector3 at, Quaternion rotation)
        {
            var prefab = await Load<GameObject>(assetReference);
            return _prefabSpawner.Spawn(prefab, at, rotation);
        }

        public async UniTask<T> Instantiate<T>(string address) where T : Component
        {
            var prefab = await Load<T>(address);
            return _prefabSpawner.Spawn(prefab);
        }

        public async UniTask<T> Instantiate<T>(string address, Vector3 at, Quaternion rotation) where T : Component
        {
            var prefab = await Load<T>(address);
            return _prefabSpawner.Spawn(prefab, at, rotation);
        }

        public async UniTask<T> Instantiate<T>(AssetReference assetReference) where T : Component
        {
            var prefab = await Load<T>(assetReference);
            return _prefabSpawner.Spawn(prefab);
        }

        public async UniTask<T> Instantiate<T>(AssetReference assetReference, Vector3 at, Quaternion rotation) where T : Component
        {
            var prefab = await Load<T>(assetReference);
            return _prefabSpawner.Spawn(prefab, at, rotation);
        }

        public void CleanUp()
        {
            foreach (AddressableData resourceHandles in _addressableData.Values)
            foreach (AsyncOperationHandle handle in resourceHandles.Handles)
                Addressables.Release(handle);

            _addressableData.Clear();
        }

        private async UniTask<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
        {
            AddHandle(cacheKey, handle);
            var result = await handle.ToUniTask();
            _addressableData[cacheKey].Ready = true;
            return result;
        }

        private void AddHandle<T>(string key, AsyncOperationHandle<T> handle) where T : class
        {
            if (!_addressableData.TryGetValue(key, out AddressableData resourceHandles))
            {
                resourceHandles = new AddressableData
                {
                    Handles = new List<AsyncOperationHandle>(),
                    Ready = false
                };

                _addressableData[key] = resourceHandles;
            }

            resourceHandles.Handles.Add(handle);
        }

        private static T GetHandlesResult<T>(AddressableData handle) where T : class =>
            handle.Handles.First().Result as T;
    }
}
