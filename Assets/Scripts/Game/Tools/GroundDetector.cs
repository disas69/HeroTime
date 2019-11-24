using System.Collections.Generic;
using UnityEngine;

namespace Game.Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class GroundDetector : MonoBehaviour
    {
        private const string GroundLayer = "Ground";

        private bool _isActive;
        private int _groundMask;
        private int _groundLayer;
        private bool _isGrounded;
        private float _groundDistance;
        private List<Collision2D> _groundCollisions;

        [SerializeField] private int _slopeLimit = 30;
        [SerializeField] private float _minGroundDistance;

        public bool IsGrounded => _isGrounded;
        public float GroundDistance => _groundDistance;

        private void Awake()
        {
            _isActive = true;
            _groundMask = LayerMask.GetMask(GroundLayer);
            _groundLayer = LayerMask.NameToLayer(GroundLayer);
            _groundCollisions = new List<Collision2D>(3);
        }

        private void Update()
        {
            if (!_isActive)
            {
                return;
            }

            UpdateGroundedStatus();
        }

        private void UpdateGroundedStatus()
        {
            var groundCollision = GetValidGroundCollision();
            if (groundCollision != null)
            {
                _groundDistance = 0f;
                _isGrounded = true;
            }
            else
            {
                _isGrounded = IsGroundInPosition(transform.position, out _groundDistance);
            }
        }

        private Collision2D GetValidGroundCollision()
        {
            for (var i = 0; i < _groundCollisions.Count; i++)
            {
                var collision = _groundCollisions[i];
                for (var j = 0; j < collision.contacts.Length; j++)
                {
                    var contact = collision.contacts[j];
                    var angle = Vector3.Angle(contact.normal, Vector3.up);

                    if (angle <= _slopeLimit)
                    {
                        return collision;
                    }
                }
            }

            return null;
        }

        private bool IsGroundInPosition(Vector2 position, out float groundDistance)
        {
            const float verticalOffset = 1f;
            const float rayLength = 10f;

            var hit2D = Raycast(position, verticalOffset, rayLength);
            groundDistance = hit2D.collider != null ? hit2D.distance - verticalOffset : rayLength;
            Debug.DrawLine(position, position + Vector2.down * groundDistance, Color.white, 1f);

            return groundDistance <= _minGroundDistance;
        }

        private RaycastHit2D Raycast(Vector2 position, float verticalOffset, float rayLength)
        {
            position = position + Vector2.up * verticalOffset;
            return Physics2D.Raycast(position, Vector2.down, rayLength, _groundMask);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_isActive)
            {
                CheckGroundCollision(collision);
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (_isActive)
            {
                CheckGroundCollision(collision);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (_isActive)
            {
                _groundCollisions.Remove(collision);
            }
        }

        private void CheckGroundCollision(Collision2D collision)
        {
            if (collision.collider != null && collision.collider.gameObject.layer == _groundLayer &&
                !_groundCollisions.Contains(collision))
            {
                _groundCollisions.Add(collision);
            }
        }
    }
}