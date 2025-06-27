using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Services
{
    /// <summary>
    /// Simple dependency injection container for managing services
    /// </summary>
    public class ServiceContainer : IServiceContainer
    {
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        private readonly Dictionary<Type, Type> _serviceTypes = new Dictionary<Type, Type>();

        public void Register<TInterface, TImplementation>()
            where TImplementation : class, TInterface, new()
            where TInterface : class
        {
            var interfaceType = typeof(TInterface);
            var implementationType = typeof(TImplementation);

            if (_serviceTypes.ContainsKey(interfaceType))
            {
                Debug.LogWarning($"[ServiceContainer] Service {interfaceType.Name} is already registered. Overwriting.");
            }

            _serviceTypes[interfaceType] = implementationType;
            
            // Remove existing instance if any
            if (_services.ContainsKey(interfaceType))
            {
                _services.Remove(interfaceType);
            }
        }

        public void Register<TInterface>(TInterface instance)
            where TInterface : class
        {
            var interfaceType = typeof(TInterface);

            if (_services.ContainsKey(interfaceType))
            {
                Debug.LogWarning($"[ServiceContainer] Service instance {interfaceType.Name} is already registered. Overwriting.");
            }

            _services[interfaceType] = instance;
            
            // Remove type registration if any
            if (_serviceTypes.ContainsKey(interfaceType))
            {
                _serviceTypes.Remove(interfaceType);
            }
        }

        public TInterface Get<TInterface>()
            where TInterface : class
        {
            var interfaceType = typeof(TInterface);

            // Check if we have a cached instance
            if (_services.TryGetValue(interfaceType, out var cachedInstance))
            {
                return cachedInstance as TInterface;
            }

            // Check if we have a type registration
            if (_serviceTypes.TryGetValue(interfaceType, out var implementationType))
            {
                try
                {
                    var instance = Activator.CreateInstance(implementationType) as TInterface;
                    _services[interfaceType] = instance;
                    return instance;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[ServiceContainer] Failed to create instance of {implementationType.Name}: {ex.Message}");
                    throw;
                }
            }

            throw new InvalidOperationException($"[ServiceContainer] Service of type {interfaceType.Name} is not registered.");
        }

        public bool IsRegistered<TInterface>()
            where TInterface : class
        {
            var interfaceType = typeof(TInterface);
            return _services.ContainsKey(interfaceType) || _serviceTypes.ContainsKey(interfaceType);
        }

        public void Unregister<TInterface>()
            where TInterface : class
        {
            var interfaceType = typeof(TInterface);
            _services.Remove(interfaceType);
            _serviceTypes.Remove(interfaceType);
        }

        public void Clear()
        {
            _services.Clear();
            _serviceTypes.Clear();
        }
    }
}
