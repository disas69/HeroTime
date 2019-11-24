using Game.Data;
using Game.Levels;
using Game.Characters;
using Game.Dimension;
using Game.Time;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class GameSession : MonoBehaviour
    {
        private Level _level;

        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private SceneManager _scene;
        [SerializeField] private Player _player;

        public void Initialize(int level)
        {
            if (_level != null)
            {
                _level.Dispose();
                Destroy(_level.gameObject);
            }
            
            TimeController.Instance.Reset();
            TimeController.Instance.Activate(false);
            DimensionController.Instance.Reset();

            _level = CreateLevel(level);
            _level.transform.SetParent(_scene.transform);
            _level.Initialize();
            _level.gameObject.SetActive(false);

            _player.transform.position = _level.Start.position;
            _player.Activate(false);
            
            _camera.transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y + 2.5f,
                _camera.transform.position.z);
        }

        public void Play()
        {
            _level.gameObject.SetActive(true);
            _player.Activate(true);
            TimeController.Instance.Activate(true);
        }

        public void Finish()
        {
            Stop();
        }

        public void Stop()
        {
            _player.Activate(false);
        }

        private Level CreateLevel(int level)
        {
            var levelConfig = GameConfiguration.GetLevelConfiguration(level);
            if (levelConfig != null)
            {
                return Instantiate(levelConfig.Prefab);
            }

            return null;
        }
    }
}