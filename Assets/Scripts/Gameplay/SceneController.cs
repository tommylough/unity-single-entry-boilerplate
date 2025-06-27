using UnityEngine;
using Core.Events;
using Core.Services;

namespace Gameplay
{
    /// <summary>
    /// Base class for scene controllers that handle scene-specific logic
    /// </summary>
    public abstract class SceneController : MonoBehaviour
    {
        [Header("Scene Controller")]
        [SerializeField] protected bool enableDebugLogs = true;

        protected IEventBus EventBus { get; private set; }
        protected IServiceContainer ServiceContainer { get; private set; }

        protected virtual void Awake()
        {
            // Get core services
            ServiceContainer = ServiceLocator.Get<IServiceContainer>();
            EventBus = ServiceLocator.Get<IEventBus>();

            if (enableDebugLogs)
                Debug.Log($"[{GetType().Name}] Awake - Services initialized.");
        }

        protected virtual void Start()
        {
            // Subscribe to events
            SubscribeToEvents();

            if (enableDebugLogs)
                Debug.Log($"[{GetType().Name}] Started and subscribed to events.");
        }

        protected virtual void OnDestroy()
        {
            // Unsubscribe from events to prevent memory leaks
            UnsubscribeFromEvents();

            if (enableDebugLogs)
                Debug.Log($"[{GetType().Name}] Destroyed and unsubscribed from events.");
        }

        /// <summary>
        /// Override this method to subscribe to events
        /// </summary>
        protected abstract void SubscribeToEvents();

        /// <summary>
        /// Override this method to unsubscribe from events
        /// </summary>
        protected abstract void UnsubscribeFromEvents();

        /// <summary>
        /// Utility method to get a service
        /// </summary>
        protected T GetService<T>() where T : class
        {
            return ServiceContainer.Get<T>();
        }
    }
}
