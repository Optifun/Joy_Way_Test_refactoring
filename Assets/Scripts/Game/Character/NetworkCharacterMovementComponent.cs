using JoyWay.Services;
using Mirror;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace JoyWay.Game.Character
{
    public class NetworkCharacterMovementComponent : NetworkBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField]
        private NetworkCharacterLookComponent _lookComponent;

        private float _maxSpeed;
        private float _movementForce;
        private float _jumpForce;
        private float _groundDrag;
        private float _airDrag;

        private const float GroundRaycastLength = 0.2f;

        private Vector3 _moveDirection;
        private Transform _cameraTransform;
        private bool _isGrounded;
        private CameraService _cameraService;

        public void Setup(float maxSpeed, float movementForce, float jumpForce, float groundDrag, float airDrag)
        {
            _maxSpeed = maxSpeed;
            _movementForce = movementForce;
            _jumpForce = jumpForce;
            _groundDrag = groundDrag;
            _airDrag = airDrag;
        }

        private void Initialize(CameraService cameraService)
        {
            _cameraService = cameraService;
            _cameraTransform = cameraService.GetCameraTransform();
        }

        [Client]
        public void Move(Vector2 moveDirection)
        {
            _moveDirection = InputDirectionToCameraLookDirection(moveDirection);
            CmdPerformMove(_moveDirection);
        }

        public void Jump()
        {
            CmdPerformJump();
        }

        [Command]
        private void CmdPerformMove(Vector3 direction)
        {
            ApplyDrag();
            _rigidbody.AddForce(direction * _movementForce, ForceMode.Force);
            ClampMovement();
        }

        [Command]
        private void CmdPerformJump()
        {
            if (CheckGrounded())
                _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }

        [Server]
        private void ApplyDrag()
        {
            if (CheckGrounded())
                _rigidbody.drag = _groundDrag;
            else
                _rigidbody.drag = _airDrag;
        }

        [Server]
        private void ClampMovement()
        {
            var velocity = _rigidbody.velocity;
            Vector3 flatMovement = new Vector3(velocity.x, 0, velocity.z);
            Vector3 clamped = Vector3.ClampMagnitude(flatMovement, _maxSpeed);
            _rigidbody.velocity = new Vector3(clamped.x, velocity.y, clamped.z);
        }

        private Vector3 InputDirectionToCameraLookDirection(Vector2 inputDirection)
        {
            Vector3 calibrationVector =
                _cameraTransform.right * inputDirection.x +
                _cameraTransform.forward * inputDirection.y;
            calibrationVector.y = 0;
            return calibrationVector.normalized;
        }

        private bool CheckGrounded()
        {
            Ray rayToGround = new Ray(transform.position + Vector3.up * GroundRaycastLength / 2, -transform.up);
            bool isGrounded = Physics.Raycast(rayToGround, GroundRaycastLength);
            return isGrounded;
        }
    }
}
