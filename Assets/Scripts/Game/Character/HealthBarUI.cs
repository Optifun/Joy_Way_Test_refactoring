using TMPro;
using UnityEngine;

namespace JoyWay.Game.Character
{
    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _healthLabel;

        public void SetHealth(int health, int maxHealth)
        {
            _healthLabel.text = $"{health}/{maxHealth}";
        }
    }
}