using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Services.GameState
{
    /// <summary>
    /// Game state service implementation for managing overall game state
    /// </summary>
    public class GameStateService : IGameStateService
    {
        private GameState _currentState = GameState.Loading;
        private GameState _previousState = GameState.Loading;

        public GameState CurrentState => _currentState;
        public bool IsPaused => _currentState == GameState.Paused;

        public event Action<GameState, GameState> OnStateChanged;
        public event Action OnGamePaused;
        public event Action OnGameResumed;

        public async Task InitializeAsync()
        {
            Debug.Log("[GameStateService] Initialized.");
            await Task.CompletedTask;
        }

        public void ChangeState(GameState newState)
        {
            if (_currentState == newState)
            {
                return; // No change needed
            }

            var oldState = _currentState;
            _previousState = _currentState;
            _currentState = newState;

            Debug.Log($"[GameStateService] State changed from {oldState} to {newState}");

            // Handle time scale for pause/resume
            UpdateTimeScale();

            // Trigger events
            OnStateChanged?.Invoke(oldState, newState);

            if (newState == GameState.Paused)
            {
                OnGamePaused?.Invoke();
            }
            else if (oldState == GameState.Paused && newState != GameState.Paused)
            {
                OnGameResumed?.Invoke();
            }
        }

        public void PauseGame()
        {
            if (_currentState != GameState.Paused)
            {
                ChangeState(GameState.Paused);
            }
        }

        public void ResumeGame()
        {
            if (_currentState == GameState.Paused)
            {
                ChangeState(_previousState);
            }
        }

        private void UpdateTimeScale()
        {
            switch (_currentState)
            {
                case GameState.Paused:
                    Time.timeScale = 0f;
                    break;
                case GameState.Loading:
                    Time.timeScale = 0f;
                    break;
                default:
                    Time.timeScale = 1f;
                    break;
            }
        }
    }
}
