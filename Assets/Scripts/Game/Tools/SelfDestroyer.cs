using Framework.Extensions;
using UnityEngine;

namespace Game.Tools
{
    public class SelfDestroyer : MonoBehaviour
    {
        public float Time;

        private void Start()
        {
            this.WaitForSeconds(Time, () =>
            {
                Destroy(gameObject);
            });
        }
    }
}