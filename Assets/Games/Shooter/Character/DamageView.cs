using System.Collections;
using UnityEngine;

namespace JoyWay.Game.Character
{
    public class DamageView : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        private float _displayDamageTakenDelay;

        public void Setup(float displayDamageTakenDelay)
        {
            _displayDamageTakenDelay = displayDamageTakenDelay;
        }

        public void DisplayDamageTaken()
        {
            StartCoroutine(CharacterReddening());
        }

        private IEnumerator CharacterReddening()
        {
            var material = _meshRenderer.material;
            material.color = Color.red;
            yield return new WaitForSeconds(_displayDamageTakenDelay);
            material.color = Color.white;
        }
    }
}