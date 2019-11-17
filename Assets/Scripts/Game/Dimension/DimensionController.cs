using System.Collections;
using System.Collections.Generic;
using Framework.Tools.Singleton;
using UnityEngine;

namespace Game.Dimension
{
    public enum Dimension
    {
        Good,
        Evil
    }

    public class DimensionController : MonoSingleton<DimensionController>
    {
        private readonly List<DimensionBehaviour> _dimensionBehaviours = new List<DimensionBehaviour>();

        private Dimension _dimension = Dimension.Good;
        private Coroutine _changeCoroutine;

        [SerializeField] private float _changeTime;
        [SerializeField] private AnimationCurve _changeCurve;
        [SerializeField] private CanvasGroup _overlay;
        [SerializeField] private Camera _camera;

        public Dimension Dimension => _dimension;

        public void Change()
        {
            Change(_dimension == Dimension.Good ? Dimension.Evil : Dimension.Good);
        }

        public void Add(DimensionBehaviour dimensionBehaviour)
        {
            if (_dimensionBehaviours.Contains(dimensionBehaviour))
            {
                return;
            }

            dimensionBehaviour.Apply(_dimension);
            _dimensionBehaviours.Add(dimensionBehaviour);
        }

        public void Remove(DimensionBehaviour dimensionBehaviour)
        {
            _dimensionBehaviours.Remove(dimensionBehaviour);
        }

        public void Reset()
        {
            Change(Dimension.Good);
        }
        
        private void Change(Dimension dimension)
        {
            _dimension = dimension;
            
            if (_changeCoroutine != null)
            {
                StopCoroutine(_changeCoroutine);
            }

            _changeCoroutine = StartCoroutine(ApplyDimension(dimension));
        }

        private IEnumerator ApplyDimension(Dimension dimension)
        {
            _overlay.alpha = 1f;
            
            if (dimension == Dimension.Good)
            {
                _camera.backgroundColor = Color.white;
            }
            else
            {
                _camera.backgroundColor = Color.black;
            }

            foreach (var dimensionBehaviour in _dimensionBehaviours)
            {
                dimensionBehaviour.Apply(dimension);
            }

            var time = 0f;

            while (time < _changeTime)
            {
                _overlay.alpha = Mathf.Lerp(1f, 0f, time / _changeTime);
                time += UnityEngine.Time.unscaledDeltaTime;
                yield return null;
            }
            
            _overlay.alpha = 0f;
            _changeCoroutine = null;
        }
    }
}