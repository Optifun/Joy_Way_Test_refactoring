﻿using System;
using Cinemachine;
using JoyWay.Infrastructure;
using UnityEngine;
using Zenject;

namespace JoyWay.Services
{
    public class CameraService : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _fpsCamera;
        [SerializeField] private Camera _camera;
        private bool _fpsCameraEnabled;

        [Inject]
        public void Construct(AdvancedNetworkManager networkManager, PlayerInputs inputs)
        {
            networkManager.ClientConnected += () => SetFpsCamera(true);
            networkManager.ClientDisconnected += () => SetFpsCamera(false);
            inputs.Character.Escape.performed += _ => SetFpsCamera(!_fpsCameraEnabled);
        }

        private void SetFpsCamera(bool value)
        {
            LockCursor(value && Application.isFocused);
            _fpsCamera.enabled = value;
            _fpsCameraEnabled = value;
        }

        private void LockCursor(bool value)
        {
            Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
        }

        public void SetFollowTarget(Transform targetTransform)
        {
            _fpsCamera.Follow = targetTransform;
        }

        public Transform GetCameraTransform()
        {
            return _camera.transform;
        }

        public Vector3 GetLookDirection()
        {
            return _camera.transform.forward;
        }
    }
}
