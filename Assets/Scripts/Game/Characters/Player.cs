using Framework.Extensions;
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
        private readonly int _velocityHash = Animator.StringToHash("Velocity");

        private bool _isActive;
        private bool _isRewinding;
        private Rigidbody2D _rigidbody2D;
        private GroundDetector _groundDetector;
        private PlayerInput _playerInput;
        private Vector2 _velocity;
        private Vector2 _gravity;
        private bool _isJumping;

        [SerializeField] private GameObject _view;
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _deathAnimation;
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
            _view.SetActive(isActive);
            gameObject.SetActive(isActive);
        }

        public void MoveOnPerformed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (context.ReadValue<Vector2>().x > 0f)
                {
                    _velocity = Vector2.right;
                    gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                }
                else
                {
                    _velocity = Vector2.left;
                    gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
                }
            }
            else if (context.canceled)
            {
                _velocity = Vector2.zero;
            }
        }

        public void JumpOnPerformed(InputAction.CallbackContext context)
        {
            if (context.started && _groundDetector.IsGrounded && !_isJumping)
            {
                _isJumping = true;
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
                const float minYVelocity = 0.001f;

                if (_velocity.x > 0 || _velocity.x < 0 || _isJumping ||
                    !_groundDetector.IsGrounded &&
                    (_rigidbody2D.velocity.y > minYVelocity || _rigidbody2D.velocity.y < -minYVelocity))
                {
                    TimeController.Instance.Play();
                }
                else
                {
                    TimeController.Instance.Stop();
                }
            }
            
            _animator.SetFloat(_velocityHash, Mathf.Abs(_velocity.x));
        }

        private void FixedUpdate()
        {
            if (!_isActive)
            {
                return;
            }

            if (_isJumping)
            {
                _rigidbody2D.AddForce(Vector2.up * GameConfiguration.PlayerSettings.JumpForce, ForceMode2D.Impulse);
                _isJumping = false;
            }
            else
            {
                Vector3 targetVelocity =
                    new Vector2(
                        _velocity.x * 10f * GameConfiguration.PlayerSettings.MoveSpeed *
                        UnityEngine.Time.fixedDeltaTime,
                        _rigidbody2D.velocity.y);
                _rigidbody2D.velocity = targetVelocity;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var fallZone = other.gameObject.GetComponent<DamageZone>();
            if (fallZone != null)
            {
                Die();
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            CheckCollision(other);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            CheckCollision(other);
        }

        private void CheckCollision(Collision2D other)
        {
            if (DimensionController.Instance.Dimension == Dimension.Dimension.Evil)
            {
                var enemy = other.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    Die();
                    return;
                }
            }

            var damageZone = other.gameObject.GetComponent<DamageZone>();
            if (damageZone != null)
            {
                Die();
            }
        }

        private void Die()
        {
            Instantiate(_deathAnimation, transform.position, Quaternion.identity);
            _onPlayedDied.Invoke();
        }

        private void OnDisable()
        {
            _playerInput.actions.Disable();
        }
    }
}