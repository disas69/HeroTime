using Game.Dimension;
using Game.Time;
using UnityEngine;

namespace Game.Elements
{
    [RequireComponent(typeof(TimeBehaviour))]
    public class Box : MonoBehaviour
    {
        private TimeBehaviour _timeBehaviour;

        private void Awake()
        {
            _timeBehaviour = GetComponent<TimeBehaviour>();
            DimensionController.Instance.DimensionChanged += OnDimensionChanged;
        }

        private void OnDimensionChanged(Dimension.Dimension dimension)
        {
            if (dimension == Dimension.Dimension.Good)
            {
                _timeBehaviour.SetInitialIsKinematic(false);
            }
            else
            {
                _timeBehaviour.SetInitialIsKinematic(true);
            }
        }

        private void OnDestroy()
        {
            DimensionController.Instance.DimensionChanged -= OnDimensionChanged;
        }
    }
}