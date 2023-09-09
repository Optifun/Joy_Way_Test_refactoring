using System;
using JoyWay.Services;
using Mirror;
using UnityEngine;
using Zenject;

namespace JoyWay.Game.Character
{
    public class NetworkCharacterLookComponent : NetworkBehaviour
    {
        public event Action<Vector3> LookDirectionChanged;

        [SerializeField] private Transform _eyes;

        [SyncVar(hook = nameof(SetLookDirection))]
        private Vector3 _lookDirection;
        
        private FPSCameraService _fpsCameraService;

        public Vector3 LookDirection { get; private set; }
        private Vector3 _newLookDirection; // TODO: replace with _lookDirection?
        private float _interpolationTimeInterval;
        private float _timer;

        [Inject]
        public void Initialize(FPSCameraService fpsCameraService)
        {
            _fpsCameraService = fpsCameraService;
        }

        public void Setup(float interpolationTimeInterval)
        {
            _interpolationTimeInterval = interpolationTimeInterval;
        }

        public void AttachCamera()
        {
            _fpsCameraService.SetFollowTarget(_eyes);
        }

        private void Update()
        {
            if (isOwned)
            {
                LookDirection = _fpsCameraService.GetLookDirection();
                UpdateLookDirection(LookDirection);
                // _lookDirection = LookDirection;
            }
            else
            {
                UpdateLookDirectionByInterpolation();
            }
        }

        private void UpdateLookDirection(Vector3 direction)
        {
            LookDirectionChanged?.Invoke(direction);
            CmdChangeLookDirection(direction);
        }

        [Command]
        private void CmdChangeLookDirection(Vector3 direction)
        {
            _lookDirection = direction;
        }

        private void UpdateLookDirectionByInterpolation()
        {
            _timer += Time.deltaTime;

            LookDirection = Vector3.Lerp(LookDirection, _newLookDirection, _timer / _interpolationTimeInterval);

            if (LookDirection != _newLookDirection)
            {
                LookDirectionChanged?.Invoke(LookDirection);
            }
        }

        private void SetLookDirection(Vector3 oldLookDirection, Vector3 newLookDirection)
        {
            _timer = 0;
            _newLookDirection = newLookDirection;
        }

        public Transform GetCameraTransform()
        {
            return _fpsCameraService.GetCameraTransform();
        }
    }
}