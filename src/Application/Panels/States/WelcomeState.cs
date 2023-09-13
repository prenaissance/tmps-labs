using Journal.Application.Panels.Abstractions;
using Journal.Application.Panels.Options;
using Journal.Application.Panels.States.Abstractions;
using Journal.Application.Views;
using Journal.DataAccess.JournalEntries;

namespace Journal.Application.Panels.States;

public class WelcomeState : IPanelState
{
    private readonly IPanelController _panelController;
    private void HandleMemoryStorageOption()
    {
        JournalRepositoryContext.JournalEntriesRepository = new MemoryJournalEntriesRepository();
        _panelController.ChangeState(
            new MenuState(_panelController)
        );
    }
    private void HandleFileStorageOption()
    {
        throw new NotImplementedException();
    }
    private readonly OptionsHandler _optionsHandler;
    public WelcomeState(IPanelController panelController)
    {
        _panelController = panelController;
        _optionsHandler = new OptionsBuilder()
            .AddOption("In memory", HandleMemoryStorageOption)
            .AddOption("In file storage (WIP)", HandleFileStorageOption)
            .Build();
    }

    public void HandleOption(string option)
    {
        try
        {
            _optionsHandler.HandleOption(option);
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e.Message);
            _panelController.Loop();
        }
    }

    public void Render()
    {
        Console.WriteLine("Welcome to the Journal Application!");
        Console.WriteLine("You can make journal entries, tag them and some other operations.");
        Console.WriteLine("Please select where you want to store your journal entries:");
        new OptionMenuView(_optionsHandler.Options, HandleOption).Render();
    }
}