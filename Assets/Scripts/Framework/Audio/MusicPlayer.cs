﻿using System.Collections.Generic;
using Framework.Audio.Configuration;
using UnityEngine;

namespace Framework.Audio
{
    [RequireComponent(typeof(AudioPlayer))]
    public class MusicPlayer : MonoBehaviour
    {
        private AudioPlayer _audioPlayer;
        private List<AudioClip> _audioClips;
        private int _clipIndex;

        [SerializeField] private AudioStorage _audioStorage;
        [SerializeField] private bool _shuffle;
        [SerializeField] private bool _playOnAwake;

        private void Awake()
        {
            _audioPlayer = GetComponent<AudioPlayer>();
            _audioClips = new List<AudioClip>(_audioStorage.Items.Count);

            for (var i = 0; i < _audioStorage.Items.Count; i++)
            {
                _audioClips.Add(_audioStorage.Items[i].AudioClip);
            }

            if (_shuffle && _audioClips.Count > 0)
            {
                Shuffle(_audioClips);
            }

            if (_playOnAwake)
            {
                Play();
            }
        }

        public void Play()
        {
            var audioClip = GetNextMusicClip();
            if (audioClip != null)
            {
                _audioPlayer.Play(audioClip);
            }
        }

        public void Stop()
        {
            var audioSource = GetAudioSource();
            if (audioSource != null)
            {
                audioSource.Stop();
            }
        }

        private void Update()
        {
            var audioSource = GetAudioSource();
            if (audioSource == null || !audioSource.isPlaying)
            {
                _clipIndex++;
                Play();
            }
        }

        private AudioClip GetNextMusicClip()
        {
            if (_clipIndex >= _audioClips.Count)
            {
                _clipIndex = 0;
            }

            if (_audioClips.Count > 0)
            {
                return _audioClips[_clipIndex];
            }

            return null;
        }

        private AudioSource GetAudioSource()
        {
            if (_audioClips.Count == 0 || _clipIndex >= _audioClips.Count)
            {
                return null;
            }

            return _audioPlayer.GetAudioSource(_audioClips[_clipIndex]);
        }

        private static void Shuffle(List<AudioClip> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                var k = Random.Range(0, n) % n;
                n--;
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}