using Game.Time;
using UnityEngine;

namespace Game.Characters
{
    [RequireComponent(typeof(Rigidbody2D), typeof(TimeBehaviour), typeof(GroundDetector))]
    public class Enemy : MonoBehaviour
    {
        private bool _targetFound;
        private Rigidbody2D _rigidbody2D;
        private Vector2 _targetDirection;

        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _searchRadius;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            var overlap = Physics2D.OverlapCircle(transform.position, _searchRadius, LayerMask.GetMask("Player"));
            if (overlap != null && overlap.gameObject.activeSelf)
            {
                _targetFound = true;
                _targetDirection = (overlap.transform.position - transform.position).normalized;
            }
            else
            {
                _targetFound = false;
            }
        }

        private void FixedUpdate()
        {
            if (TimeController.Instance.IsReversed || !_targetFound)
            {
                return;
            }

            Vector3 targetVelocity =
                new Vector2(_targetDirection.x * 10f * _moveSpeed * UnityEngine.Time.fixedDeltaTime,
                    _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = targetVelocity;
        }
    }
}