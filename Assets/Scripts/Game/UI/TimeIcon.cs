using Game.Dimension;
using UnityEngine;

namespace Game.UI
{
    public class TimeIcon : MonoBehaviour
    {
        [SerializeField] private GameObject _goodView;
        [SerializeField] private GameObject _evilView;

        private void Awake()
        {
            OnDimensionChanged(DimensionController.Instance.Dimension);
            DimensionController.Instance.DimensionChanged += OnDimensionChanged;
        }

        private void OnDimensionChanged(Dimension.Dimension dimension)
        {
            if (dimension == Dimension.Dimension.Good)
            {
                _goodView.SetActive(true);
                _evilView.SetActive(false);
            }
            else
            {
                _goodView.SetActive(false);
                _evilView.SetActive(true);
            }
        }

        private void OnDestroy()
        {
            DimensionController.Instance.DimensionChanged -= OnDimensionChanged;
        }
    }
}