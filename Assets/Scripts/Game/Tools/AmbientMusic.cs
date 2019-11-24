using UnityEngine;

namespace Game.Tools
{
    [RequireComponent(typeof(AudioSource))]
    public class AmbientMusic : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            _audioSource.Play();
        }

        private void OnDisable()
        {
            _audioSource.Pause();
        }
    }
}