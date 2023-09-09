using UnityEngine;
using Zenject;
namespace JoyWay.Game.Character
{
    public class CharacterHeadRotation : MonoBehaviour
    {
        [SerializeField] private Transform _shouldersHeightTransform;

        [SerializeField]
        private NetworkCharacterLookComponent _lookComponent;
        private Vector3 _cachedDirection;

        private void Update()
        {
            if (_lookComponent == null || _cachedDirection == _lookComponent.LookDirection)
                return;

            _cachedDirection = _lookComponent.LookDirection;
            _shouldersHeightTransform.forward = _cachedDirection;
            transform.forward = new Vector3(_cachedDirection.x, 0, _cachedDirection.z);
        }
    }
}
