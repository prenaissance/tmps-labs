using Journal.Application.Panels.Abstractions;
using Journal.Application.Panels.Options;
using Journal.Application.Panels.States.Abstractions;
using Journal.DataAccess.JournalEntries;

namespace Journal.Application.Panels.States;

public class WelcomeState : IPanelState
{
    private readonly IPanelController _panelController;
    private void HandleMemoryStorageOption()
    {
        _panelController.ChangeState(
            new MenuState(
                _panelController,
                new MemoryJournalEntriesRepository())
        );
    }
    private readonly OptionsHandler _optionsHandler;
    public WelcomeState(IPanelController panelController)
    {
        _panelController = panelController;
        _optionsHandler = new OptionsBuilder()
            .AddOption(1, "In memory", HandleMemoryStorageOption)
            .Build();
    }

    public void HandleOption(int optionNumber)
    {
        try
        {
            _optionsHandler.HandleOption(optionNumber);
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e.Message);
            _panelController.Loop();
        }
    }

    public void PrintMenu()
    {
        Console.WriteLine("Welcome to the Journal Application!");
        Console.WriteLine("You can make journal entries, tag them and some other operations.");
        Console.WriteLine("Please select where you want to store your journal entries:");
        Console.WriteLine(_optionsHandler.OptionsMenu);
        Console.WriteLine("... Other WIP");
    }
}