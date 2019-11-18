using Game.Time;
using UnityEngine;

namespace Game.Characters
{
    [RequireComponent(typeof(Rigidbody2D), typeof(TimeBehaviour), typeof(GroundDetector))]
    public class Enemy : MonoBehaviour
    {
        private bool _isTargetFound;
        private Rigidbody2D _rigidbody2D;
        private Vector2 _targetDirection;
        private float _spawnXPosition;
        private bool _isGoingLeft;

        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _chaseSpeed;
        [SerializeField] private float _searchRadius;
        [SerializeField] private float _patrolRadius;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spawnXPosition = transform.position.x;
        }

        private void Update()
        {
            var overlap = Physics2D.OverlapCircle(transform.position, _searchRadius, LayerMask.GetMask("Player"));
            if (overlap != null && overlap.gameObject.activeSelf)
            {
                _isTargetFound = true;
                _targetDirection = (overlap.transform.position - transform.position).normalized;
            }
            else
            {
                _isTargetFound = false;
                
                if (_isGoingLeft)
                {
                    if (transform.position.x > _spawnXPosition - _patrolRadius / 2f)
                    {
                        _targetDirection = Vector2.left;
                    }
                    else
                    {
                        _targetDirection = Vector2.right;
                        _isGoingLeft = false;
                    }
                }
                else
                {
                    if (transform.position.x < _spawnXPosition + _patrolRadius / 2f)
                    {
                        _targetDirection = Vector2.right;
                    }
                    else
                    {
                        _targetDirection = Vector2.left;
                        _isGoingLeft = true;
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            if (TimeController.Instance.IsReversed)
            {
                return;
            }

            var speed = _isTargetFound ? _chaseSpeed : _moveSpeed;
            Vector3 targetVelocity = new Vector2(_targetDirection.x * 10f * speed * UnityEngine.Time.fixedDeltaTime,
                    _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = targetVelocity;
        }
    }
}