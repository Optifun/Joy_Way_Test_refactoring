﻿using TMPro;
using UnityEngine;

namespace JoyWay.Game.Character
{
    public class CharacterHealthBarUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _healthLabel;

        public void Initialize(int health, int maxHealth)
        {
            SetHealth(health, maxHealth);
        }

        public void SetHealth(int health, int maxHealth)
        {
            _healthLabel.text = $"{health}/{maxHealth}";
        }
    }
}