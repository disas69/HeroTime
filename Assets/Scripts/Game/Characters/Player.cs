using Game.Data;
using Game.Dimension;
using Game.Elements;
using Game.Time;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game.Characters
{
    [RequireComponent(typeof(Rigidbody2D), typeof(GroundDetector), typeof(PlayerInput))]
    public class Player : MonoBehaviour
    {
        private bool _isActive;
        private bool _isRewinding;
        private Rigidbody2D _rigidbody2D;
        private GroundDetector _groundDetector;
        private PlayerInput _playerInput;
        private Vector2 _velocity;
        private Vector2 _gravity;

        [SerializeField] private UnityEvent _onPlayedDied;

        private void Awake()
        {
            _isActive = false;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _groundDetector = GetComponent<GroundDetector>();
            _playerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            _playerInput.actions.Enable();
        }

        public void Activate(bool isActive)
        {
            _isActive = isActive;
            gameObject.SetActive(isActive);
        }

        public void MoveOnPerformed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _velocity = context.ReadValue<Vector2>().x > 0f ? Vector2.right : Vector2.left;
            }
            else if (context.canceled)
            {
                _velocity = Vector2.zero;
            }
        }

        public void JumpOnPerformed(InputAction.CallbackContext context)
        {
            if (context.started && _groundDetector.IsGrounded)
            {
                _rigidbody2D.AddForce(Vector2.up * GameConfiguration.PlayerSettings.JumpForce, ForceMode2D.Impulse);
            }
        }

        public void DimensionOnPerformed(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                DimensionController.Instance.Change();
            }
        }

        public void RewindOnPerformed(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _isRewinding = true;
            }
            else if (context.canceled)
            {
                _isRewinding = false;
            }
        }

        private void Update()
        {
            if (!_isActive)
            {
                return;
            }

            if (_isRewinding && TimeController.Instance.CanRewind)
            {
                TimeController.Instance.Reverse();
            }
            else
            {
                if (_velocity.x > 0 || _velocity.x < 0 || _rigidbody2D.velocity.y > 0f ||
                    _rigidbody2D.velocity.y < 0f)
                {
                    TimeController.Instance.Play();
                }
                else
                {
                    TimeController.Instance.Stop();
                }
            }
        }

        private void FixedUpdate()
        {
            if (!_isActive)
            {
                return;
            }

            Vector3 targetVelocity =
                new Vector2(_velocity.x * 10f * GameConfiguration.PlayerSettings.MoveSpeed * UnityEngine.Time.fixedDeltaTime,
                    _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = targetVelocity;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var fallZone = other.gameObject.GetComponent<DamageZone>();
            if (fallZone != null)
            {
                _onPlayedDied.Invoke();
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                _onPlayedDied.Invoke();
            }
        }

        private void OnDisable()
        {
            _playerInput.actions.Disable();
        }
    }
}