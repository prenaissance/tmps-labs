using Journal.Application.Panels.Abstractions;
using Journal.Application.Panels.States.Abstractions;

namespace Journal.Application.Panels.States;

public class WelcomeState : IPanelState
{
    private readonly IPanelController _panelController;
    public WelcomeState(IPanelController panelController)
    {
        _panelController = panelController;
    }
    public void HandleOption(int optionNumber)
    {
        throw new NotImplementedException();
    }

    public void PrintMenu()
    {
        Console.WriteLine("Welcome to the Journal Application!");
        Console.WriteLine("You can make journal entries, tag them and some other operations.");
        Console.WriteLine("Please select an option");
    }
}