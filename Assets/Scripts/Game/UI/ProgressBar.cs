using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _fillerImage;

        public void Activate(bool value)
        {
            gameObject.SetActive(value);
        }

        public void Set(float value)
        {
            _fillerImage.fillAmount = value;
        }
    }
}