using UnityEngine;

namespace Game.Parallax
{
    public sealed class ParallaxBehavior : MonoBehaviour
    {
        [SerializeField] private float _parallaxEffect = 1f;

        private float _startPosition;
        private Vector3 _velocity;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            _startPosition = transform.position.x;
        }

        private void LateUpdate()
        {
            var dist = _camera.transform.position.x * _parallaxEffect;
            var targetPosition = new Vector3(_startPosition + dist, transform.position.y, transform.position.z);
            transform.position = targetPosition;
        }
    }
}