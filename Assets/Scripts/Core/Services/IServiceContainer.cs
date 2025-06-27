namespace Core.Services
{
    /// <summary>
    /// Interface for dependency injection container
    /// </summary>
    public interface IServiceContainer
    {
        /// <summary>
        /// Register a service implementation for an interface
        /// </summary>
        void Register<TInterface, TImplementation>()
            where TImplementation : class, TInterface, new()
            where TInterface : class;

        /// <summary>
        /// Register a service instance for an interface
        /// </summary>
        void Register<TInterface>(TInterface instance)
            where TInterface : class;

        /// <summary>
        /// Get a service instance by interface type
        /// </summary>
        TInterface Get<TInterface>()
            where TInterface : class;

        /// <summary>
        /// Check if a service is registered
        /// </summary>
        bool IsRegistered<TInterface>()
            where TInterface : class;

        /// <summary>
        /// Unregister a service
        /// </summary>
        void Unregister<TInterface>()
            where TInterface : class;

        /// <summary>
        /// Clear all registered services
        /// </summary>
        void Clear();
    }
}
