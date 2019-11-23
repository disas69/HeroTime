using Game.Time;
using UnityEngine;

namespace Game.UI
{
    public class TimeIconController : MonoBehaviour
    {
        public GameObject Play;
        public GameObject Stop;
        public GameObject Rewind;

        private void Update()
        {
            if (TimeController.Instance.IsPlaying)
            {
                if (TimeController.Instance.IsReversed)
                {
                    Play.SetActive(false);
                    Stop.SetActive(false);
                    Rewind.SetActive(true);
                }
                else
                {
                    Play.SetActive(true);
                    Stop.SetActive(false);
                    Rewind.SetActive(false);
                }
            }
            else
            {
                Play.SetActive(false);
                Stop.SetActive(true);
                Rewind.SetActive(false);
            }
        }
    }
}