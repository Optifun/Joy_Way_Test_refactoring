using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JoyWay.Services
{
    public class SceneLoader : MonoBehaviour
    {
        public Coroutine Load(string name, Action onLoaded = null) => StartCoroutine((LoadScene(name, onLoaded)));

        private IEnumerator LoadScene(string nextScene, Action onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == nextScene)
            {
                onLoaded?.Invoke();
                yield break;
            }

            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            while (!waitNextScene.isDone)
                yield return null;

            onLoaded?.Invoke();
        }
    }
}