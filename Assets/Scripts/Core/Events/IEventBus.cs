using System;

namespace Core.Events
{
    /// <summary>
    /// Interface for event bus system providing type-safe event handling
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Subscribe to an event type with a callback
        /// </summary>
        void Subscribe<T>(Action<T> callback) where T : IGameEvent;

        /// <summary>
        /// Unsubscribe from an event type with a specific callback
        /// </summary>
        void Unsubscribe<T>(Action<T> callback) where T : IGameEvent;

        /// <summary>
        /// Unsubscribe all callbacks for an event type
        /// </summary>
        void UnsubscribeAll<T>() where T : IGameEvent;

        /// <summary>
        /// Publish an event to all subscribers
        /// </summary>
        void Publish<T>(T gameEvent) where T : IGameEvent;

        /// <summary>
        /// Clear all event subscriptions
        /// </summary>
        void Clear();

        /// <summary>
        /// Get the number of subscribers for an event type
        /// </summary>
        int GetSubscriberCount<T>() where T : IGameEvent;
    }
}
