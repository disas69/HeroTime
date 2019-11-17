using Framework.Tools.Gameplay;
using Framework.Tools.Singleton;
using Framework.UI;
using Game.Data;
using Game.UI;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(GameSession))]
    public class GameController : MonoSingleton<GameController>
    {
        private GameSession _gameSession;
        private StateMachine<GameState> _stateMachine;

        public GameSession Session => _gameSession;
        public GameState State => _stateMachine.CurrentState;

        protected override void Awake()
        {
            base.Awake();
            _gameSession = GetComponent<GameSession>();
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            GameData.Load();
            CreateStateMachine();
            ActivateStartState();
        }

        public void SetState(int state)
        {
            SetState((GameState) state);
        }

        public void SetState(GameState gameState)
        {
            _stateMachine.SetState(gameState);
        }

        private void CreateStateMachine()
        {
            _stateMachine = new StateMachine<GameState>(GameState.Start);
            _stateMachine.AddTransition(GameState.Start, GameState.Play, ActivatePlayState);
            _stateMachine.AddTransition(GameState.Play, GameState.End, ActivateEndState);
            _stateMachine.AddTransition(GameState.End, GameState.Start, ActivateStartState);
        }

        private void ActivateStartState()
        {
            _gameSession.Initialize(GameData.Data.Level);
            NavigationManager.Instance.OpenScreen<StartPage>();
        }

        private void ActivatePlayState()
        {
            _gameSession.Play();
            NavigationManager.Instance.OpenScreen<PlayPage>();
        }

        private void ActivateEndState()
        {
            _gameSession.Stop();
            NavigationManager.Instance.OpenScreen<EndPage>();
            GameData.Save();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                GameData.Save();
            }
        }
    }
}