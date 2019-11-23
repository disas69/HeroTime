using System.Collections.Generic;
using Framework.Tools.Singleton;
using Framework.UnityEvents;
using UnityEngine;

namespace Game.Time
{
    public class TimeController : MonoSingleton<TimeController>
    {
        private readonly LinkedList<TimeState> _timeStates = new LinkedList<TimeState>();
        private readonly List<TimeBehaviour> _timeBehaviours = new List<TimeBehaviour>();

        private bool _isActive;
        private bool _isPlaying;
        private bool _isReversed;
        private float _targetTime;
        private float _time;

        [SerializeField] private float _recordTime;
        [SerializeField] private AnimationCurve _changeTimeCurve;
        [SerializeField] private UnityFloatEvent _onTimeChange;

        public bool CanRewind => _timeStates.Count > 0;
        public bool IsReversed => _isReversed;
        public bool IsPlaying => _isPlaying;

        protected override void Awake()
        {
            base.Awake();
            Play();
        }

        public void Activate(bool isActive)
        {
            _isActive = isActive;
        }

        public void Add(TimeBehaviour timeBehaviour)
        {
            if (_timeBehaviours.Contains(timeBehaviour))
            {
                return;
            }
            
            _timeBehaviours.Add(timeBehaviour);
        }

        public void Remove(TimeBehaviour timeBehaviour)
        {
            _timeBehaviours.Remove(timeBehaviour);
        }

        public void Reset()
        {
            _time = 0f;
            _timeStates.Clear();
            Play();
        }

        public void Play()
        {
            _targetTime = 1f;
            _isPlaying = true;
            _isReversed = false;

            ActivateTimeBehaviours(false);
        }

        public void Stop()
        {
            _targetTime = 0f;
            _isPlaying = false;
            
            ActivateTimeBehaviours(true);
        }

        public void Reverse()
        {
            _targetTime = 1f;
            _isPlaying = true;
            _isReversed = true;

            ActivateTimeBehaviours(true);
        }

        private void ActivateTimeBehaviours(bool isActive)
        {
            foreach (var timeBehaviour in _timeBehaviours)
            {
                timeBehaviour.Activate(isActive);
            }
        }

        private void Update()
        {
            UnityEngine.Time.timeScale = _targetTime;
        }

        private void FixedUpdate()
        {
            if (!_isActive || !_isPlaying)
            {
                return;
            }

            if (_isReversed)
            {
                Rewind();
            }
            else
            {
                Record();
            }
        }

        private void Rewind()
        {
            if (_timeStates.Count > 0)
            {
                var timeState = _timeStates.First.Value;

                foreach (var timeBehaviour in _timeBehaviours)
                {
                    var state = timeState.GetState(timeBehaviour);
                    if (state != null)
                    {
                        timeBehaviour.Apply(state);
                    }
                }
                
                _timeStates.RemoveFirst();
                _time = Mathf.Max(0f, _time -= UnityEngine.Time.fixedDeltaTime);
            }
            else
            {
                Stop();
            }
            
            _onTimeChange.Invoke(_time / _recordTime);
        }

        private void Record()
        {
            _time = Mathf.Min(_recordTime, _time += UnityEngine.Time.fixedDeltaTime);

            if(_time / _recordTime >= 1f)
            {
                _timeStates.RemoveLast();
            }

            _timeStates.AddFirst(new TimeState(_timeBehaviours));
            _onTimeChange.Invoke(_time / _recordTime);
        }
    }
}