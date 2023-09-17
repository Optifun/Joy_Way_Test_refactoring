using JoyWay.Core.Resources;
using UnityEngine;
namespace JoyWay.Core.Utils
{
    public class LevelSpawnPoints : MonoBehaviour
    {
        private GameObject[] _charactersSpawnPoints;

        public GameObject[] GetSpawnPoints()
        {
            return GameObject.FindGameObjectsWithTag(CoreResources.SpawnPointTag);
        }

        public Transform GetRandomSpawnPoint()
        {
            if (_charactersSpawnPoints == null)
                _charactersSpawnPoints = GetSpawnPoints();

            int index = Random.Range(0, _charactersSpawnPoints.Length);

            return _charactersSpawnPoints[index].transform;
        }
    }
}
