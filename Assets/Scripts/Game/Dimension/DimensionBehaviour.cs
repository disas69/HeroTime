using UnityEngine;

namespace Game.Dimension
{
    public class DimensionBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject _goodView;
        [SerializeField] private GameObject _evilView;

        private void Awake()
        {
            DimensionController.Instance.Add(this);
        }

        public void Apply(Dimension dimension)
        {
            if (dimension == Dimension.Good)
            {
                if (_evilView != null)
                {
                    _evilView.SetActive(false);
                }

                if (_goodView != null)
                {
                    _goodView.SetActive(true);
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                if (_goodView != null)
                {
                    _goodView.SetActive(false);
                }

                if (_evilView != null)
                {
                    _evilView.SetActive(true);
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }

        private void OnDestroy()
        {
            DimensionController.Instance.Remove(this);
        }
    }
}