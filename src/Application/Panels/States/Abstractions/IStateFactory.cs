using Journal.Domain.Models;

namespace Journal.Application.Panels.States.Abstractions;

public interface IStateFactory
{
    public StateT CreateState<StateT>() where StateT : IPanelState;
    public StateT CreateState<StateT>(JournalEntry dependentEntry) where StateT : IPanelState;
}