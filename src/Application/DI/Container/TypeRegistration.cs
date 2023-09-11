using Journal.Application.DI.Container.Enums;

namespace Journal.Application.DI.Container;

public record TypeRegistration(Type InterfaceType, Type ImplementationType, InjectionType InjectionType);