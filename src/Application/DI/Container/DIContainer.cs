using Journal.Application.DI.Container.Enums;

namespace Journal.Application.DI.Container;

public class DIContainer
{
    private readonly Dictionary<Type, TypeRegistration> _typeRegistrations = new();
    private readonly Dictionary<Type, object> _singletonInstances = new();
    public DIContainer RegisterSingleton<TInterface, TImplementation>() where TImplementation : TInterface
    {
        _typeRegistrations[typeof(TInterface)] = new TypeRegistration(
            typeof(TInterface),
            typeof(TImplementation),
            InjectionType.Singleton);

        return this;
    }
    public DIContainer RegisterSingleton<TImplementation>() where TImplementation : class
    {
        _typeRegistrations[typeof(TImplementation)] = new TypeRegistration(
            typeof(TImplementation),
            typeof(TImplementation),
            InjectionType.Singleton);

        return this;
    }
    public DIContainer RegisterSingleton<TInterface>(TInterface instance) where TInterface : notnull
    {
        _singletonInstances[typeof(TInterface)] = instance;
        _typeRegistrations[typeof(TInterface)] = new TypeRegistration(
            typeof(TInterface),
            typeof(TInterface),
            InjectionType.Singleton);

        return this;
    }
    public DIContainer RegisterTransient<TInterface, TImplementation>() where TImplementation : TInterface
    {
        _typeRegistrations[typeof(TInterface)] = new TypeRegistration(
            typeof(TInterface),
            typeof(TImplementation),
            InjectionType.Transient);

        return this;
    }

    public DIContainer RegisterTransient<TImplementation>() where TImplementation : class
    {
        _typeRegistrations[typeof(TImplementation)] = new TypeRegistration(
            typeof(TImplementation),
            typeof(TImplementation),
            InjectionType.Transient);

        return this;
    }

    public TInterface Resolve<TInterface>()
    {
        return (TInterface)Resolve(typeof(TInterface));
    }

    private object Resolve(Type type)
    {
        var typeRegistration = _typeRegistrations.GetValueOrDefault(type);
        if (typeRegistration == null)
        {
            throw new InvalidOperationException($"Type {type.Name} is not registered.");
        }

        if (typeRegistration.InjectionType == InjectionType.Singleton)
        {
            if (!_singletonInstances.ContainsKey(type))
            {
                var constructor = typeRegistration.ImplementationType.GetConstructors()[0];
                _singletonInstances[type] = constructor.Invoke(
                    constructor.GetParameters()
                        .Select(parameter => Resolve(parameter.ParameterType))
                        .ToArray());
            }

            return _singletonInstances[type];
        }

        var implementationConstructor = typeRegistration.ImplementationType.GetConstructors()[0];
        return implementationConstructor.Invoke(
            implementationConstructor.GetParameters()
                .Select(parameter => Resolve(parameter.ParameterType))
                .ToArray());
    }
}