using Journal.Application.Panels.Abstractions;
using Journal.Application.Panels.States.Abstractions;

namespace Journal.Application.Panels;

public class PanelController : IPanelController
{
    public IPanelState? CurrentState { private get; set; }
    public void ChangeState(IPanelState newState)
    {
        CurrentState = newState;
        Loop();
    }

    public void Loop()
    {
        if (CurrentState == null)
        {
            throw new InvalidOperationException("Initial state not initiated");
        }
        CurrentState.PrintMenu();
        var option = Console.ReadLine();
        if (int.TryParse(option, out var optionNumber))
        {
            CurrentState.HandleOption(optionNumber);
        }
        else
        {
            Console.WriteLine("Invalid option, please try again");
            Loop();
        }
    }
}