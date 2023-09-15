namespace Journal.Application.Panels.States.Abstractions;

public interface IStateFactory
{
    public StateT CreateState<StateT>() where StateT : IPanelState;
}