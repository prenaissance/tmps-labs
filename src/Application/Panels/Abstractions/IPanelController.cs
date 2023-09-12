using Journal.Application.Panels.States.Abstractions;

namespace Journal.Application.Panels.Abstractions;

public interface IPanelController
{
    public IPanelState? CurrentState { set; }
    public void Loop();
    public void ChangeState(IPanelState newState);
}