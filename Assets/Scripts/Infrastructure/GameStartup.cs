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
            _gameFlow.StartGame();
        }
    }
}