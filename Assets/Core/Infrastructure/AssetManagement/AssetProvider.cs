using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Zenject;

namespace JoyWay.Core.Infrastructure.AssetManagement
{
    public class AssetProvider : IAssets, IInitializable, IDisposable
    {
        private readonly Dictionary<string, AddressableData> _addressableData = new Dictionary<string, AddressableData>();
        private readonly List<Action> _releaseResourcesActions = new();
        private readonly PrefabSpawner _prefabSpawner;
        private readonly ILogger<AssetProvider> _logger;

        public AssetProvider(PrefabSpawner prefabSpawner, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AssetProvider>();
            _prefabSpawner = prefabSpawner;
        }

        public async void Initialize()
        {
            IResourceLocator locator = await Addressables.InitializeAsync();
            _logger.LogTrace("Loaded addressable paths");
            _logger.LogTrace("{Keys}", ZString.Join("\n\r", locator.Keys));
        }

        public void Dispose()
        {
            CleanUp();
        }

        public async UniTask<T> Load<T>(AssetReference assetReference) where T : class
        {
            if (_addressableData.TryGetValue(assetReference.AssetGUID, out AddressableData handle) && handle.Ready)
                return GetHandlesResult<T>(handle);


            return await RunWithCacheOnComplete( //TODO: почему c <T>  не грузит префаб, если он определяется адресаблами
                Addressables.LoadAssetAsync<T>(assetReference),
                cacheKey: assetReference.AssetGUID);
        }

        public async UniTask<T> Load<T>(string key) where T : class
        {
            if (_addressableData.TryGetValue(key, out AddressableData handle) && handle.Ready)
                return GetHandlesResult<T>(handle);

            return await RunWithCacheOnComplete(
                Addressables.LoadAssetAsync<T>(key),
                cacheKey: key);
        }


        public UniTask<SceneInstance> LoadScene(string path, LoadSceneMode loadSceneMode = LoadSceneMode.Single, bool activateOnLoad = true)
        {
            return Addressables.LoadSceneAsync(path, loadSceneMode, activateOnLoad).ToUniTask();
        }

        public async UniTask<T> Load<T>(ICollection<string> keys) where T : class
        {
            AsyncOperationHandle<IList<T>> operationHandle = Addressables.LoadAssetsAsync<T>(keys, OnLoaded,
                Addressables.MergeMode.UseFirst, true);

            void OnLoaded(T obj)
            {
            }

            _releaseResourcesActions.Add(() =>
            {
                Addressables.Release(operationHandle);
            });

            var resources = await operationHandle.ToUniTask();
            return resources.First();
        }

        public async UniTask<IList<T>> LoadMultiple<T>(IEnumerable<string> keys, bool matchAny) where T : class
        {
            AsyncOperationHandle<IList<T>> operationHandle = Addressables.LoadAssetsAsync<T>(keys, OnLoaded,
                matchAny ? Addressables.MergeMode.Union : Addressables.MergeMode.Intersection, true);

            void OnLoaded(T obj)
            {
            }

            _releaseResourcesActions.Add(() =>
            {
                Addressables.Release(operationHandle);
            });

            var resources = await operationHandle.ToUniTask();
            return resources;
        }

        public async UniTask<GameObject> Instantiate(string key)
        {
            var prefab = await Load<GameObject>(key);
            return _prefabSpawner.Spawn(prefab);
        }

        public async UniTask<GameObject> Instantiate(string key, Vector3 at, Quaternion rotation)
        {
            var prefab = await Load<GameObject>(key);
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

        public async UniTask<T> Instantiate<T>(string key) where T : Component
        {
            var prefab = await Load<T>(key);
            return _prefabSpawner.Spawn(prefab);
        }

        public async UniTask<T> Instantiate<T>(string key, Vector3 at, Quaternion rotation) where T : Component
        {
            var prefab = await Load<T>(key);
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

            foreach (var action in _releaseResourcesActions)
            {
                action.Invoke();
            }

            _releaseResourcesActions.Clear();
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
