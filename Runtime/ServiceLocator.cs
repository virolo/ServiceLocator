using System;
using System.Collections.Generic;

namespace xenxei
{
    public class ServiceLocator
    {
        private static readonly Dictionary<Type, Dictionary<Enum, object>> _registry = new();

        private static ServiceLocator _instance;

        public static ServiceLocator Instance => _instance ??= new ServiceLocator();

        public void Register<TInterface, TImplementation>() where TImplementation : TInterface, new()
        {
            Register<TInterface, TImplementation>(ServiceLocatorContext.General);
        }

        public void Register<TInterface, TImplementation>(Enum context) where TImplementation : TInterface, new()
        {
            Register<TInterface>(context, new TImplementation());
        }

        public void Register<T>(T service)
        {
            Register(ServiceLocatorContext.General, service);
        }

        public void Register<T>(Enum context, T service)
        {
            Type type = typeof(T);
            if (!_registry.ContainsKey(type))
            {
                _registry[type] = new Dictionary<Enum, object>();
            }

            _registry[type][context] = service;
        }

        public T Resolve<T>()
        {
            return Resolve<T>(ServiceLocatorContext.General);
        }

        public T Resolve<T>(Enum context)
        {
            Type type = typeof(T);
            if (_registry.ContainsKey(type) && _registry[type].ContainsKey(context))
            {
                return (T)_registry[type][context];
            }

            throw new Exception($"No registered instance found for {type} with context '{context}'");
        }
    }
}