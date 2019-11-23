using Game.Time;
using UnityEngine;

namespace Game.Elements
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class FloatingPlatform : MonoBehaviour
    {
        private bool _isFloating;
        private Rigidbody2D _rigidbody2D;
        private Vector2 _initialPosition;

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
            if (!_isFloating || TimeController.Instance.IsReversed || !TimeController.Instance.IsPlaying)
            {
                return;
            }

            var delta = Vector2.Distance(_initialPosition, _rigidbody2D.position);
            if (delta < _maxDelta)
            {
                _rigidbody2D.velocity = new Vector2(0, _speed * UnityEngine.Time.fixedDeltaTime) * 10f;
            }
        }
    }
}