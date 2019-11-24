using UnityEngine;

namespace Game.Tools
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraShake : MonoBehaviour
    {
        private float _time = 0;
        private Camera _camera;
        private Vector3 _lastPos;
        private Vector3 _nextPos;
        private float _lastFoV;
        private float _nextFoV;

        public Vector3 Amount = new Vector3(1f, 1f, 0);
        public float Duration = 1;
        public float Speed = 10;
        public AnimationCurve Curve = AnimationCurve.EaseInOut(0, 1, 1, 0);
        public bool DeltaMovement = true;

        private void Awake()
        {
            _camera = GetComponent<UnityEngine.Camera>();
        }

        public void Shake()
        {
            ResetCam();
            _time = Duration;
        }

        private void LateUpdate()
        {
            if (_time > 0)
            {
                _time -= UnityEngine.Time.unscaledDeltaTime;

                if (_time > 0)
                {
                    _nextPos = (Mathf.PerlinNoise(_time * Speed, _time * Speed * 2) - 0.5f) * Amount.x *
                               transform.right *
                               Curve.Evaluate(1f - _time / Duration) +
                               (Mathf.PerlinNoise(_time * Speed * 2, _time * Speed) - 0.5f) * Amount.y * transform.up *
                               Curve.Evaluate(1f - _time / Duration);
                    _nextFoV = (Mathf.PerlinNoise(_time * Speed * 2, _time * Speed * 2) - 0.5f) * Amount.z *
                               Curve.Evaluate(1f - _time / Duration);

                    _camera.fieldOfView += (_nextFoV - _lastFoV);
                    _camera.transform.Translate(DeltaMovement ? (_nextPos - _lastPos) : _nextPos);

                    _lastPos = _nextPos;
                    _lastFoV = _nextFoV;
                }
                else
                {
                    ResetCam();
                }
            }
        }

        private void ResetCam()
        {
            _camera.transform.Translate(DeltaMovement ? -_lastPos : Vector3.zero);
            _camera.fieldOfView -= _lastFoV;

            _lastPos = _nextPos = Vector3.zero;
            _lastFoV = _nextFoV = 0f;
        }
    }
}