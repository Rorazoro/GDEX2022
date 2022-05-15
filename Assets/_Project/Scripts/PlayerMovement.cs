using System;
using Photon.Pun;
using UnityEngine;

namespace _Project.Scripts
{
    [RequireComponent(typeof(Player))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerMovement : MonoBehaviourPun
    {
        private static readonly int IsFalling = Animator.StringToHash("IsFalling");
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int InputMagnatude = Animator.StringToHash("InputMagnitude");
        private static readonly int InputH = Animator.StringToHash("InputH");
        private static readonly int InputV = Animator.StringToHash("InputV");

        [SerializeField] private Vector3 _velocity;
        [SerializeField] private float _rotationVelocity;

        public float rotationSpeed;
        public float movementSpeed;
        public float jumpHeight;
        public float gravityMultiplier;
        public float jumpHorizontalSpeed;
        public float jumpButtonGracePeriod;
        public float animationLayerSmoothTime;
        
        private const float Threshold = 0.01f;

        private Player _player;
        private Animator _animator;
        private CharacterController _characterController;
        private PlayerInputHandler _inputHandler;
        private bool _isGrounded;
        private bool _isJumping;
        private float? _jumpButtonPressedTime;
        private float? _lastGroundedTime;
        private float _originalStepOffset;
        private float _ySpeed;
        private float _yAnimVelocity;
        private static readonly int MovementSpeed = Animator.StringToHash("MovementSpeed");

        private void Awake()
        {
            _player = GetComponent<Player>();
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();
            _inputHandler = GetComponent<PlayerInputHandler>();
            _originalStepOffset = _characterController.stepOffset;
        }

        private void Update()
        {
            if (!photonView.IsMine)
                return;

            Jump();
        }

        private void FixedUpdate()
        {
            if (!photonView.IsMine)
                return;

            Move();
        }

        private void OnAnimatorMove()
        {
            if (!_isGrounded) return;

            _velocity = _animator.deltaPosition;
            _velocity.y = _ySpeed * Time.deltaTime;

            _characterController.Move(_velocity);
        }

        private void Jump()
        {
            var jumpHold = _inputHandler.playerActionMap.JumpHold;
            var jump = _inputHandler.playerActionMap.Jump;

            var gravity = Physics.gravity.y * gravityMultiplier;

            if (_isJumping && _ySpeed > 0 && !jumpHold) gravity *= 2;

            _ySpeed += gravity * Time.deltaTime;

            if (_characterController.isGrounded) _lastGroundedTime = Time.time;

            if (jump) _jumpButtonPressedTime = Time.time;

            if (Time.time - _lastGroundedTime <= jumpButtonGracePeriod)
            {
                _characterController.stepOffset = _originalStepOffset;
                _ySpeed = -0.5f;
                _animator.SetBool(IsGrounded, true);
                _isGrounded = true;
                _animator.SetBool(IsJumping, false);
                _isJumping = false;
                _animator.SetBool(IsFalling, false);

                if (Time.time - _jumpButtonPressedTime <= jumpButtonGracePeriod)
                {
                    _ySpeed = Mathf.Sqrt(jumpHeight * -3 * gravity);
                    _animator.SetBool(IsJumping, true);
                    _isJumping = true;
                    _jumpButtonPressedTime = null;
                    _lastGroundedTime = null;
                }

                _animator.applyRootMotion = true;
            }
            else
            {
                _animator.applyRootMotion = false;
                _characterController.stepOffset = 0;
                _animator.SetBool(IsGrounded, false);
                _isGrounded = false;

                if ((_isJumping && _ySpeed < 0) || _ySpeed < -2) _animator.SetBool(IsFalling, true);
            }

            _velocity.y = _ySpeed;
            _characterController.Move(_velocity * Time.deltaTime);
        }

        private void Move()
        {
            var hInput = _inputHandler.playerActionMap.Move.x;
            var vInput = _inputHandler.playerActionMap.Move.y;
            var sprint = _inputHandler.playerActionMap.Sprint;

            var movementDirection = new Vector3(hInput, 0, vInput);
            var inputMagnatude = Mathf.Clamp01(movementDirection.magnitude);

            //Check if player is running
            if (!sprint) inputMagnatude /= 2;

            _animator.SetFloat(InputMagnatude, inputMagnatude, 0.05f, Time.deltaTime);
            _animator.SetFloat(InputH, hInput, 0.05f, Time.deltaTime);
            _animator.SetFloat(InputV, vInput, 0.05f, Time.deltaTime);
            _animator.SetFloat(MovementSpeed, movementSpeed);
            
            if (inputMagnatude > 0)
            {
                movementDirection =
                    Quaternion.AngleAxis(_player.playerCamera.transform.rotation.eulerAngles.y, Vector3.up) *
                    movementDirection;
                movementDirection.Normalize();
            }

            var toRotation = Quaternion.LookRotation(_player.playerCamera.transform.forward);
            toRotation.x = 0f;
            toRotation.z = 0f;
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            
            if (movementDirection != Vector3.zero)
            {
                _animator.SetBool(IsMoving, true);
            }
            else
            {
                _animator.SetBool(IsMoving, false);
            }

            if (_isGrounded) return;
            
            _velocity = movementDirection * (inputMagnatude * jumpHorizontalSpeed);
        }

        // private void SetLayerWeightSmooth(int layerIndex, float layerWeight)
        // {
        //     float currWeight = _animator.GetLayerWeight(layerIndex);
        //     float startWeight = Mathf.SmoothDamp(currWeight, layerWeight, ref _yAnimVelocity, animationLayerSmoothTime);
        //     _animator.SetLayerWeight(layerIndex, startWeight);
        // }
    }
}