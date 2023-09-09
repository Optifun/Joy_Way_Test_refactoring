using JoyWay.Core.Components;
using UnityEngine;
namespace JoyWay.Games.Shooter.Interactable
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
