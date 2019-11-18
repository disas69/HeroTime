using Game.Characters;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Elements
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TriggerZone : MonoBehaviour
    {
        public UnityEvent OnEnter;
        public UnityEvent OnExit;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.GetComponent<Player>();
            if (player != null)
            {
                OnEnter.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var player = other.GetComponent<Player>();
            if (player != null)
            {
                OnExit.Invoke();
            }
        }
    }
}