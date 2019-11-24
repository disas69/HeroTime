using Game.Dimension;
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
        private int _groundMask;

        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _chaseSpeed;
        [SerializeField] private float _searchRadius;
        [SerializeField] private float _patrolRadius;
        [SerializeField] private float _raycastLength = 1f;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spawnXPosition = transform.position.x;
            _groundMask = LayerMask.GetMask("Ground");
        }

        private void Update()
        {
            if (DimensionController.Instance.Dimension == Dimension.Dimension.Evil)
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
                    Patrol();
                }
            }
            else
            {
                _isTargetFound = false;
                Patrol();
            }
        }

        private void Patrol()
        {
            if (_isGoingLeft)
            {
                var ray = Physics2D.Raycast(transform.position, Vector2.left, _raycastLength, _groundMask);

                if (transform.position.x > _spawnXPosition - _patrolRadius / 2f && ray.collider == null)
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
                var ray = Physics2D.Raycast(transform.position, Vector2.right, _raycastLength, _groundMask);
                
                if (transform.position.x < _spawnXPosition + _patrolRadius / 2f && ray.collider == null)
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

        private void FixedUpdate()
        {
            if (TimeController.Instance.IsReversed)
            {
                return;
            }

            if (_targetDirection.x > 0f)
            {
                gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            }

            var speed = _isTargetFound ? _chaseSpeed : _moveSpeed;
            Vector3 targetVelocity = new Vector2(_targetDirection.x * 10f * speed * UnityEngine.Time.fixedDeltaTime,
                _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = targetVelocity;
        }
    }
}