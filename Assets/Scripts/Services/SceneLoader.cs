using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JoyWay.Services
{
    public class SceneLoader
    {
        public async UniTask LoadAsync(string sceneName, Action onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                onLoaded?.Invoke();
                return;
            }
            await SceneManager.LoadSceneAsync(sceneName).ToUniTask();
            onLoaded?.Invoke();
        }

        public void Load(string sceneName, Action onLoaded = null) => LoadAsync(sceneName, onLoaded).Forget();
    }
}