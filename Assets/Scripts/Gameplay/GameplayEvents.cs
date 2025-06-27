using UnityEngine;
using Core.Events;

namespace Gameplay
{
    /// <summary>
    /// Player-related events
    /// </summary>
    public class PlayerSpawnedEvent : IGameEvent
    {
        public Vector3 SpawnPosition { get; }
        public GameObject PlayerObject { get; }

        public PlayerSpawnedEvent(Vector3 position, GameObject player)
        {
            SpawnPosition = position;
            PlayerObject = player;
        }
    }

    public class PlayerDiedEvent : IGameEvent
    {
        public Vector3 DeathPosition { get; }
        public GameObject PlayerObject { get; }

        public PlayerDiedEvent(Vector3 position, GameObject player)
        {
            DeathPosition = position;
            PlayerObject = player;
        }
    }

    public class PlayerHealthChangedEvent : IGameEvent
    {
        public int CurrentHealth { get; }
        public int MaxHealth { get; }
        public int HealthChange { get; }

        public PlayerHealthChangedEvent(int current, int max, int change)
        {
            CurrentHealth = current;
            MaxHealth = max;
            HealthChange = change;
        }
    }

    /// <summary>
    /// Level-related events
    /// </summary>
    public class LevelStartedEvent : IGameEvent
    {
        public string LevelName { get; }

        public LevelStartedEvent(string levelName)
        {
            LevelName = levelName;
        }
    }

    public class LevelCompletedEvent : IGameEvent
    {
        public string LevelName { get; }
        public float CompletionTime { get; }

        public LevelCompletedEvent(string levelName, float time)
        {
            LevelName = levelName;
            CompletionTime = time;
        }
    }

    public class LevelFailedEvent : IGameEvent
    {
        public string LevelName { get; }
        public string FailureReason { get; }

        public LevelFailedEvent(string levelName, string reason)
        {
            LevelName = levelName;
            FailureReason = reason;
        }
    }

    /// <summary>
    /// Game state related events
    /// </summary>
    public class GamePausedEvent : IGameEvent
    {
        public GamePausedEvent() { }
    }

    public class GameResumedEvent : IGameEvent
    {
        public GameResumedEvent() { }
    }

    /// <summary>
    /// Item/Collectible events
    /// </summary>
    public class ItemCollectedEvent : IGameEvent
    {
        public string ItemType { get; }
        public Vector3 CollectionPosition { get; }
        public int Quantity { get; }

        public ItemCollectedEvent(string itemType, Vector3 position, int quantity = 1)
        {
            ItemType = itemType;
            CollectionPosition = position;
            Quantity = quantity;
        }
    }

    /// <summary>
    /// UI events
    /// </summary>
    public class ShowUIEvent : IGameEvent
    {
        public string PanelName { get; }

        public ShowUIEvent(string panelName)
        {
            PanelName = panelName;
        }
    }

    public class HideUIEvent : IGameEvent
    {
        public string PanelName { get; }

        public HideUIEvent(string panelName)
        {
            PanelName = panelName;
        }
    }
}
