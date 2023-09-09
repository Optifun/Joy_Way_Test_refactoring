using JoyWay.Games.Shooter.Services;
using Mirror;
using UnityEngine;
using Zenject;
namespace JoyWay.Games.Shooter.Character
{
    public class NetworkCharacterMovementComponent : NetworkBehaviour
    {

        private const float GroundRaycastLength = 0.2f;
        [SerializeField] private Rigidbody _rigidbody;
        private float _airDrag;
        private Transform _cameraTransform;
        private float _groundDrag;
        private bool _isGrounded;
        private float _jumpForce;

        private float _maxSpeed;

        private Vector3 _moveDirection;
        private float _movementForce;

        public void Setup(float maxSpeed, float movementForce, float jumpForce, float groundDrag, float airDrag)
        {
            _maxSpeed = maxSpeed;
            _movementForce = movementForce;
            _jumpForce = jumpForce;
            _groundDrag = groundDrag;
            _airDrag = airDrag;
        }

        [Inject]
        private void Initialize(FPSCameraService fpsCameraService)
        {
            _cameraTransform = fpsCameraService.GetCameraTransform();
        }

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
            {
                var oldVelocity = _rigidbody.velocity;
                var horizontalVelocity = new Vector3(oldVelocity.x, 0, oldVelocity.z);
                _rigidbody.velocity = horizontalVelocity;
                _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            }
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
            var flatMovement = new Vector3(velocity.x, 0, velocity.z);
            var clamped = Vector3.ClampMagnitude(flatMovement, _maxSpeed);
            _rigidbody.velocity = new Vector3(clamped.x, velocity.y, clamped.z);
        }

        private Vector3 InputDirectionToCameraLookDirection(Vector2 inputDirection)
        {
            var calibrationVector =
                _cameraTransform.right * inputDirection.x +
                _cameraTransform.forward * inputDirection.y;
            calibrationVector.y = 0;
            return calibrationVector.normalized;
        }

        private bool CheckGrounded()
        {
            var rayToGround = new Ray(transform.position + Vector3.up * GroundRaycastLength / 2, -transform.up);
            bool isGrounded = Physics.Raycast(rayToGround, GroundRaycastLength);
            return isGrounded;
        }
    }
}
