using TMPro;
using UnityEngine;

namespace JoyWay.Game.Character
{
    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _healthLabel;
        private Camera _camera;

        public void Setup(Camera cam)
        {
            _camera = cam;

        }

        public void SetHealth(int health, int maxHealth)
        {
            _healthLabel.text = $"{health}/{maxHealth}";
        }

        private void Update()
        {
            transform.rotation = _camera.transform.rotation;
        }
    }
}