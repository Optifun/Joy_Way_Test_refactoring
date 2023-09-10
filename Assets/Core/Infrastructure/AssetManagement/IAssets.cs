using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace JoyWay.Core.Infrastructure.AssetManagement
{
  public interface IAssets
  {
    UniTask<T> Load<T>(AssetReference assetReference) where T : class;
    UniTask<T> Load<T>(string address) where T : class;
    void CleanUp();
    UniTask<GameObject> Instantiate(string address, Vector3 at, Quaternion rotation);
    UniTask<GameObject> Instantiate(AssetReference assetReference);
    UniTask<GameObject> Instantiate(AssetReference assetReference, Vector3 at, Quaternion rotation);
    UniTask<T> Instantiate<T>(string address) where T : Component;
    UniTask<T> Instantiate<T>(string address, Vector3 at, Quaternion rotation) where T : Component;
    UniTask<T> Instantiate<T>(AssetReference assetReference) where T : Component;
    UniTask<T> Instantiate<T>(AssetReference assetReference, Vector3 at, Quaternion rotation) where T : Component;
  }
}