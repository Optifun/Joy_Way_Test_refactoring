using Core.Components;
using UnityEngine;

namespace JoyWay.Game
{
    public class WorldButton : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject _interactableObject;
        
        public void Interact()
        {
            if (_interactableObject.TryGetComponent<IInteractable>(out var interactable))
                interactable.Interact();
        }
    }
}