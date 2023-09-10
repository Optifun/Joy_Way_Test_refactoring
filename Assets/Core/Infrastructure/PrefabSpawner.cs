using UnityEngine;
using Zenject;
using GameObject = UnityEngine.GameObject;

namespace JoyWay.Core.Infrastructure
{
    public class PrefabSpawner
    {
        private readonly DiContainer _container;

        public PrefabSpawner(DiContainer container)
        {
            _container = container;
        }

        public GameObject Spawn(GameObject prefab)
        {
            return _container.InstantiatePrefab(prefab);
        }

        public GameObject Spawn(GameObject prefab, Transform parent)
        {
            return _container.InstantiatePrefab(prefab, parent);
        }

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return _container.InstantiatePrefab(prefab, position, rotation, null);
        }

        public TComponent Spawn<TComponent>(TComponent prefab) where TComponent : Component
        {
            return _container.InstantiatePrefabForComponent<TComponent>(prefab);
        }

        public TComponent Spawn<TComponent>(TComponent prefab, Transform parent) where TComponent : Component
        {
            return _container.InstantiatePrefabForComponent<TComponent>(prefab, parent);
        }

        public TComponent Spawn<TComponent>(TComponent prefab, Vector3 position, Quaternion rotation) where TComponent : Component
        {
            return _container.InstantiatePrefabForComponent<TComponent>(prefab, position, rotation, null);
        }
    }
}
