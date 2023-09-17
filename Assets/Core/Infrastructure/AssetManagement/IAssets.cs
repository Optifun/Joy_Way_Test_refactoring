using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace JoyWay.Core.Infrastructure.AssetManagement
{
  public interface IAssets
  {
    UniTask<SceneInstance> LoadScene(string path, LoadSceneMode loadSceneMode = LoadSceneMode.Single, bool activateOnLoad = true);
    UniTask<T> Load<T>(AssetReference assetReference) where T : class;
    UniTask<T> Load<T>(string key) where T : class;
    UniTask<T> Load<T>(ICollection<string> keys) where T : class;
    UniTask<IList<T>> LoadMultiple<T>(IEnumerable<string> keys, bool matchAny) where T : class;
    void CleanUp();
    UniTask<GameObject> Instantiate(string key);
    UniTask<GameObject> Instantiate(string key, Vector3 at, Quaternion rotation);
    UniTask<GameObject> Instantiate(AssetReference assetReference);
    UniTask<GameObject> Instantiate(AssetReference assetReference, Vector3 at, Quaternion rotation);
    UniTask<T> Instantiate<T>(string key) where T : Component;
    UniTask<T> Instantiate<T>(string key, Vector3 at, Quaternion rotation) where T : Component;
    UniTask<T> Instantiate<T>(AssetReference assetReference) where T : Component;
    UniTask<T> Instantiate<T>(AssetReference assetReference, Vector3 at, Quaternion rotation) where T : Component;
  }
}