using Game.Time;
using UnityEngine;

namespace Game.Elements
{
    public class FloatingPlatform : MonoBehaviour
    {
        private bool _isFloating;
        private Vector2 _initialPosition;

        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _speed;
        [SerializeField] private float _maxDelta;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _initialPosition = _rigidbody2D.position;
        }

        public void Float()
        {
            _isFloating = true;
        }

        private void FixedUpdate()
        {
            if (!_isFloating || _maxDelta <= 0f || TimeController.Instance.IsReversed ||
                !TimeController.Instance.IsPlaying)
            {
                return;
            }

            var vector = Vector2.zero;
            var delta = Vector2.Distance(_initialPosition, _rigidbody2D.position);
            if (delta < _maxDelta)
            {
                vector = Vector2.up;
            }
            else if (delta > _maxDelta)
            {
                vector = Vector2.down;
            }

            _rigidbody2D.velocity = vector * _speed * 10f * UnityEngine.Time.fixedDeltaTime;
        }
    }
}