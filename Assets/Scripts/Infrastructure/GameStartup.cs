using JoyWay.Resources;
using JoyWay.Services;
using UnityEngine.SceneManagement;
using Zenject;

namespace JoyWay.Infrastructure
{
    public class GameStartup : IInitializable
    {
        private readonly SceneLoader _sceneLoader;
        private readonly GameFlow _gameFlow;

        public GameStartup(
            SceneLoader sceneLoader,
            GameFlow gameFlow)
        {
            _sceneLoader = sceneLoader;
            _gameFlow = gameFlow;
        }

        public void Initialize()
        {
            if (SceneManager.GetActiveScene().name != Constants.GameScene)
            {
                _sceneLoader.Load(Constants.GameScene, StartGame);
            }
            else
            {
                StartGame();
            }
        }

        private void StartGame()
        {
            _gameFlow.StartGame();
        }
    }
}