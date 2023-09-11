namespace Journal.Application.Panels.States.Abstractions;

public interface IPanelState
{
    public void PrintMenu();
    public void HandleOption(int optionNumber);
}