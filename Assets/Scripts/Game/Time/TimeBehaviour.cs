using UnityEngine;

namespace Game.Time
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class TimeBehaviour : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        private bool _isKinematicInitial;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _isKinematicInitial = _rigidbody2D.isKinematic;
        }

        private void OnEnable()
        {
            TimeController.Instance.Add(this);
        }

        public void SetInitialIsKinematic(bool value)
        {
            _isKinematicInitial = value;
        }

        public void Activate(bool isActive)
        {
            if (isActive)
            {
                _rigidbody2D.isKinematic = true;
            }
            else
            {
                _rigidbody2D.isKinematic = _isKinematicInitial;
            }

            if (isActive)
            {
                _rigidbody2D.velocity = Vector2.zero;
            }
        }

        public void Apply(State state)
        {
            _rigidbody2D.MovePosition(state.Position);
            _rigidbody2D.SetRotation(state.Rotation);
            _rigidbody2D.velocity = state.Velocity;
        }

        public State Get()
        {
            return new State
            {
                Position = _rigidbody2D.position,
                Rotation = _rigidbody2D.rotation,
                Velocity = _rigidbody2D.velocity
            };
        }

        private void OnDisable()
        {
            TimeController.Instance.Remove(this);
        }
    }
}