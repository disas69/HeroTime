using Game.Time;
using UnityEngine;

namespace Game.Effects
{
    public class DustEffect : MonoBehaviour
    {
        [SerializeField] private float _gravity;
        [SerializeField] private bool _isReversed;
        [SerializeField] private ParticleSystem _particleSystem;

        private void Update()
        {
            var velocityOverLifetime = _particleSystem.velocityOverLifetime;
            var gravity = 0f;

            if (TimeController.Instance.IsPlaying)
            {
                if (TimeController.Instance.IsReversed)
                {
                    if (_isReversed)
                    {
                        gravity = _gravity;
                    }
                    else
                    {
                        gravity = -_gravity;
                    }
                }
                else
                {
                    if (_isReversed)
                    {
                        gravity = -_gravity;
                    }
                    else
                    {
                        gravity = _gravity;
                    }
                }
            }

            velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(gravity);
        }
    }
}