using Journal.Application.DI.Container;
using Journal.Application.Panels.States.Abstractions;

namespace Journal.Application.Panels.States.Factory;

public class StateFactory : IStateFactory
{
    private readonly DIContainer _container;

    public StateFactory(DIContainer container)
    {
        _container = container;
    }

    public StateT CreateState<StateT>() where StateT : IPanelState
    {
        return _container.Resolve<StateT>();
    }
}