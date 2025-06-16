using System;
using System.Collections.Generic;

namespace Plugins.ServiceLocator
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new();
        public static IEnumerable<object> Services => _services.Values;
        private static readonly Dictionary<Type, object> _localServices = new();

        public static void Register<TService>(TService service) where TService : IService
        {
            _services.Add(typeof(TService), service);
        }

        public static TService Get<TService>() where TService : IService
        {
            return Get<TService>(_services);
        }
        
        public static void RegisterLocal<TService>(TService service) where TService : ILocalService
        {
            _localServices.Add(typeof(TService), service);
        }

        public static TService GetLocal<TService>() where TService : ILocalService
        {
            return Get<TService>(_localServices);
        }

        public static void ClearLocal()
        {
            foreach (var service in _localServices.Values)
            {
                if (service is IDisposable disposable)
                    disposable.Dispose();
            }
            _localServices.Clear();
        }
        
        private static TService Get<TService>(Dictionary<Type, object> storage)
        {
            var type = typeof(TService);
            if (storage.TryGetValue(type, out var service))
                return (TService)service;
            foreach (var kvp in storage)
            {
                if (type.IsAssignableFrom(kvp.Key))
                    return (TService)kvp.Value;
            }
            throw new Exception($"Service of type {type} is not registered.");
        }
    }
}