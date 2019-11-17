using System;
using System.Collections;
using Framework.UI.Structure.Base.Model;
using Framework.UI.Structure.Base.View;
using UnityEngine;

namespace Game.UI
{
    public class PlayPage : Page<PageModel>
    {
        [SerializeField] private CanvasGroup _overlay;
        [SerializeField] private float _overlayTransitionSpeed;

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