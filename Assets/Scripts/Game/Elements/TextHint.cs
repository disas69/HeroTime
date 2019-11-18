using System.Collections;
using UnityEngine;

namespace Game.Elements
{
    [RequireComponent(typeof(Animator))]
    public class TextHint : MonoBehaviour
    {
        private Animator _animator;
        private Coroutine _animationCoroutine;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);

            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }

            _animator.Play("Show");
        }

        public void Hide()
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }

            _animator.Play("Hide");

            if (gameObject.activeSelf)
            {
                _animationCoroutine = StartCoroutine(WaitForAnimation(_animator.GetCurrentAnimatorStateInfo(0).length));
            }
        }

        private IEnumerator WaitForAnimation(float time)
        {
            yield return new WaitForSecondsRealtime(time);
            gameObject.SetActive(false);
        }
    }
}