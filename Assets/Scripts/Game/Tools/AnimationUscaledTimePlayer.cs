using System.Linq;
using UnityEngine;

namespace Game.Tools
{
    [RequireComponent(typeof(Animation))]
    public class AnimationUscaledTimePlayer : MonoBehaviour
    {
        private float _time;
        private Animation[] _animations;
        private AnimationState[] _states;

        public bool RestartOnEnable;

        private void Awake()
        {
            _animations = GetComponentsInChildren<Animation>();
            _states = _animations.Select(anim => anim[anim.clip.name]).ToArray();
            
            if (_animations.Length == 0)
            {
                enabled = false;
            }
        }

        private void OnEnable()
        {
            if (RestartOnEnable)
            {
                _time = 0;
            }
        }

        private void LateUpdate()
        {
            _time += UnityEngine.Time.unscaledDeltaTime;
            
            for (var i = 0; i < _animations.Length; i++)
            {
                _states[i].time = _time;
                _animations[i].Sample();
            }
        }
    }
}