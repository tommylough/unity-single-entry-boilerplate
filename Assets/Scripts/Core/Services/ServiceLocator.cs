using UnityEngine;

namespace Core.Services
{
    /// <summary>
    /// Static service locator for accessing the main service container
    /// </summary>
    public static class ServiceLocator
    {
        private static IServiceContainer _container;

        /// <summary>
        /// Initialize the service locator with a container
        /// </summary>
        public static void Initialize(IServiceContainer container)
        {
            _container = container;
            Debug.Log("[ServiceLocator] Initialized with service container.");
        }

        /// <summary>
        /// Get a service from the container
        /// </summary>
        public static TInterface Get<TInterface>()
            where TInterface : class
        {
            if (_container == null)
            {
                throw new System.InvalidOperationException("[ServiceLocator] Service container is not initialized. Call Initialize() first.");
            }

            return _container.Get<TInterface>();
        }

        /// <summary>
        /// Check if a service is registered
        /// </summary>
        public static bool IsRegistered<TInterface>()
            where TInterface : class
        {
            return _container?.IsRegistered<TInterface>() ?? false;
        }

        /// <summary>
        /// Reset the service locator (useful for cleanup)
        /// </summary>
        public static void Reset()
        {
            _container = null;
            Debug.Log("[ServiceLocator] Reset completed.");
        }

        /// <summary>
        /// Check if the service locator is initialized
        /// </summary>
        public static bool IsInitialized => _container != null;
    }
}
