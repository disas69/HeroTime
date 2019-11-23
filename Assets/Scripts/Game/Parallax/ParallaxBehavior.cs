using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Parallax
{
    public sealed class ParallaxBehavior : MonoBehaviour
    {
        [SerializeField] private float _parallaxEffect = 1f;

        private float _lenght, _startPosition;
        private Camera _camera;

        private void OnEnable()
        {
            ParallaxController.Instance.AddBehavior(this);
        }

        void Start()
        {
            _camera = Camera.main;
            _startPosition = transform.position.x;
            _lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        void Update()
        {
            var dist = _camera.transform.position.x * _parallaxEffect;
            transform.position = new Vector3(_startPosition + dist, transform.position.y, transform.position.z);
        }

        private void OnDisable()
        {
            ParallaxController.Instance.RemoveBehavior(this);
        }
    }
}
