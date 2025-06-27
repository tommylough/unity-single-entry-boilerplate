using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Events
{
    /// <summary>
    /// Event bus implementation for decoupled communication between systems
    /// </summary>
    public class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _eventSubscriptions = new Dictionary<Type, List<Delegate>>();

        public void Subscribe<T>(Action<T> callback) where T : IGameEvent
        {
            var eventType = typeof(T);

            if (!_eventSubscriptions.ContainsKey(eventType))
            {
                _eventSubscriptions[eventType] = new List<Delegate>();
            }

            _eventSubscriptions[eventType].Add(callback);
        }

        public void Unsubscribe<T>(Action<T> callback) where T : IGameEvent
        {
            var eventType = typeof(T);

            if (_eventSubscriptions.TryGetValue(eventType, out var callbacks))
            {
                callbacks.Remove(callback);
                
                // Clean up empty lists
                if (callbacks.Count == 0)
                {
                    _eventSubscriptions.Remove(eventType);
                }
            }
        }

        public void UnsubscribeAll<T>() where T : IGameEvent
        {
            var eventType = typeof(T);
            _eventSubscriptions.Remove(eventType);
        }

        public void Publish<T>(T gameEvent) where T : IGameEvent
        {
            var eventType = typeof(T);

            if (_eventSubscriptions.TryGetValue(eventType, out var callbacks))
            {
                // Create a copy to avoid modification during iteration
                var callbacksCopy = new List<Delegate>(callbacks);

                foreach (var callback in callbacksCopy)
                {
                    try
                    {
                        if (callback is Action<T> typedCallback)
                        {
                            typedCallback.Invoke(gameEvent);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"[EventBus] Error invoking callback for event {eventType.Name}: {ex.Message}");
                        Debug.LogException(ex);
                    }
                }
            }
        }

        public void Clear()
        {
            _eventSubscriptions.Clear();
            Debug.Log("[EventBus] All event subscriptions cleared.");
        }

        public int GetSubscriberCount<T>() where T : IGameEvent
        {
            var eventType = typeof(T);
            return _eventSubscriptions.TryGetValue(eventType, out var callbacks) ? callbacks.Count : 0;
        }
    }
}
