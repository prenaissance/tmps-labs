using Journal.Application.Abstractions;

namespace Journal.Application.Panels.States.Abstractions;

public interface IPanelState : IView
{
    public void HandleOption(string option);
}