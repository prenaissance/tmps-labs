using Journal.Application.DI.Container;
using Journal.Application.Panels.States.Abstractions;
using Journal.Domain.Models;

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
    public StateT CreateState<StateT>(JournalEntry dependentEntry) where StateT : IPanelState
    {
        _container.RegisterSingleton(dependentEntry);
        StateT state = _container.Resolve<StateT>();
        return state;
    }
}