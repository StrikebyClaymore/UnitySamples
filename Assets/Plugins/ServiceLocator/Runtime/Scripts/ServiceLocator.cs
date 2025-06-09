using System;
using System.Collections.Generic;

namespace Plugins.ServiceLocator
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new();
        public static IEnumerable<object> Services => _services.Values;

        public static void Register<TService>(TService service) where TService : IService
        {
            _services.Add(typeof(TService), service);
        }

        public static TService Get<TService>()
        {
            var type = typeof(TService);
            if (_services.TryGetValue(type, out var service))
                return (TService)service;
            foreach (var kvp in _services)
            {
                if (type.IsAssignableFrom(kvp.Key))
                    return (TService)kvp.Value;
            }
            throw new Exception($"Service of type {type} is not registered.");
        }
    }
}