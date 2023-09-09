using Core.Components;
using Mirror;
using UnityEngine;

namespace JoyWay.Game
{
    public class LiftPlatform : NetworkBehaviour, IInteractable
    {
        [SerializeField] private float _liftingTime;
        [SerializeField] private Transform _platform;
        [SerializeField] private Transform _bottom;
        [SerializeField] private Transform _top;
        
        private bool _isRaised;
        private float _timeInterval;

        private Vector3 _startPosition;
        private Vector3 _endPosition;

        private bool _isLifting => _timeInterval < _liftingTime;

        private void Awake()
        {
            _isRaised = false;
            _timeInterval = _liftingTime;
        }

        [Server]
        public void Interact()
        {
            if (!_isLifting)
            {
                if (_isRaised)
                {
                    _startPosition = _top.position;
                    _endPosition = _bottom.position;
                }
                else
                {
                    _startPosition = _bottom.position;
                    _endPosition = _top.position;
                }
                
                _isRaised = !_isRaised;
                _timeInterval = 0;
            }
        }

        private void Update()
        {
            Lift();
        }

        private void Lift()
        {
            if (_isLifting)
            {
                _timeInterval += Time.deltaTime / _liftingTime;
                _platform.transform.position = Vector3.Lerp(_startPosition, _endPosition, _timeInterval);
            }
        }
    }
}