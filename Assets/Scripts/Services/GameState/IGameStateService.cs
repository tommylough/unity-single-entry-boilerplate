using System;

namespace Services.GameState
{
    /// <summary>
    /// Represents the current state of the game
    /// </summary>
    public enum GameState
    {
        Loading,
        Menu,
        Playing,
        Paused,
        GameOver,
        Victory
    }

    /// <summary>
    /// Interface for game state management
    /// </summary>
    public interface IGameStateService
    {
        /// <summary>
        /// Current game state
        /// </summary>
        GameState CurrentState { get; }

        /// <summary>
        /// Change the game state
        /// </summary>
        void ChangeState(GameState newState);

        /// <summary>
        /// Pause the game
        /// </summary>
        void PauseGame();

        /// <summary>
        /// Resume the game
        /// </summary>
        void ResumeGame();

        /// <summary>
        /// Check if the game is paused
        /// </summary>
        bool IsPaused { get; }

        /// <summary>
        /// Event triggered when game state changes
        /// </summary>
        event Action<GameState, GameState> OnStateChanged;

        /// <summary>
        /// Event triggered when game is paused
        /// </summary>
        event Action OnGamePaused;

        /// <summary>
        /// Event triggered when game is resumed
        /// </summary>
        event Action OnGameResumed;
    }
}
