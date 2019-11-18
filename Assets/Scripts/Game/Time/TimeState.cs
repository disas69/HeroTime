using System.Collections.Generic;
using UnityEngine;

namespace Game.Time
{
    public class State
    {
        public Vector2 Position;
        public float Rotation; 
        public Vector2 Velocity;
    }
    
    public class TimeState
    {
        private Dictionary<TimeBehaviour, State> _states;
        
        public TimeState(List<TimeBehaviour> timeBehaviours)
        {
            Record(timeBehaviours);
        }

        public State GetState(TimeBehaviour timeBehaviour)
        {
            State state;
            if (_states.TryGetValue(timeBehaviour, out state))
            {
                return state;
            }

            return null;
        }

        private void Record(List<TimeBehaviour> timeBehaviours)
        {
            _states = new Dictionary<TimeBehaviour, State>(timeBehaviours.Count);

            foreach (var timeBehaviour in timeBehaviours)
            {
                _states.Add(timeBehaviour, timeBehaviour.Get());
            }
        }
    }
}