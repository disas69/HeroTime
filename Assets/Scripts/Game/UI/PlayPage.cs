using System;
using System.Collections;
using Framework.UI.Structure.Base.Model;
using Framework.UI.Structure.Base.View;
using Game.Characters;
using UnityEngine;

namespace Game.UI
{
    public class PlayPage : Page<PageModel>
    {
        private Player _player;
        
        [SerializeField] private CanvasGroup _overlay;
        [SerializeField] private float _overlayTransitionSpeed;
        [SerializeField] private GameObject _mobileInput;

        protected override void Awake()
        {
            base.Awake();
            _mobileInput.SetActive(false);
        }

        public override void OnEnter()
        {
            base.OnEnter();

#if UNITY_EDITOR || UNITY_ANDROID
            _mobileInput.SetActive(true);
            _player = FindObjectOfType<Player>();
#endif
        }
        
        public void MoveRight()
        {
            if (_player != null)
            {
                _player.MoveRight();
            }
        }

        public void MoveLeft()
        {
            if (_player != null)
            {
                _player.MoveLeft();
            }
        }

        public void Stop()
        {
            if (_player != null)
            {
                _player.Stop();
            }
        }

        public void Jump()
        {
            if (_player != null)
            {
                _player.Jump();
            }
        }

        public void ChangeDimension()
        {
            if (_player != null)
            {
                _player.ChangeDimension();
            }
        }

        public void Rewind(bool value)
        {
            if (_player != null)
            {
                _player.Rewind(value);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            _overlay.gameObject.SetActive(false);
        }

        protected override IEnumerator InTransition(Action callback)
        {
            yield return StartCoroutine(ShowOverlay());
        }

        private IEnumerator ShowOverlay()
        {
            _overlay.gameObject.SetActive(true);
            _overlay.blocksRaycasts = true;
            _overlay.alpha = 1f;

            while (_overlay.alpha > 0f)
            {
                _overlay.alpha -= _overlayTransitionSpeed * 2f * UnityEngine.Time.unscaledDeltaTime;
                yield return null;
            }

            _overlay.alpha = 0f;
            _overlay.blocksRaycasts = false;
            _overlay.gameObject.SetActive(false);
        }
    }
}