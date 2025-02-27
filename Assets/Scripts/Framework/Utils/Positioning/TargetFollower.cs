﻿using UnityEngine;

namespace Framework.Utils.Positioning
{
    public class TargetFollower : MonoBehaviour
    {
        private Vector3 _offset;
        private Vector3 _lastCopiedPosition;
        private Vector3 _velocity;

        [SerializeField] private Transform _target;
        [SerializeField] private bool _followX;
        [SerializeField] private bool _followY;
        [SerializeField] private bool _followZ;
        [SerializeField] private float _yLimit;
        [SerializeField] private float _moveSmoothing;

        private void Awake()
        {
            if (_target != null)
            {
                _offset = transform.position - _target.transform.position;
            }
        }

        public void SetTarget(Transform target)
        {
            _target = target;
            _offset = transform.position - _target.transform.position;
        }

        private void LateUpdate()
        {
            if (_target != null && isActiveAndEnabled)
            {
                var targetPosition = _target.position + _offset;

                if (_moveSmoothing > 0)
                {
                    var newPos = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _moveSmoothing,
                        float.PositiveInfinity, Time.unscaledDeltaTime);
                    SetPosition(newPos, _followX, _followY, _followZ);
                }
                else if (targetPosition != _lastCopiedPosition)
                {
                    SetPosition(targetPosition, _followX, _followY, _followZ);
                    _lastCopiedPosition = targetPosition;
                }
            }
        }

        private void SetPosition(Vector3 position, bool followX, bool followY, bool followZ)
        {
            var newPosition = transform.position;

            if (followX)
            {
                newPosition.x = position.x;
            }

            if (followY)
            {
                if (position.y < _yLimit)
                {
                    newPosition.y = _yLimit;
                }
                else
                {
                    newPosition.y = position.y;
                }
            }

            if (followZ)
            {
                newPosition.z = position.z;
            }

            transform.position = newPosition;
        }
    }
}