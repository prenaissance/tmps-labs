using Journal.Application.Panels.Abstractions;
using Journal.Application.Panels.States.Abstractions;
using Journal.DataAccess.JournalEntries.Abstractions;

namespace Journal.Application.Panels.States;

public class MenuState : IPanelState
{
    private readonly IPanelController _panelController;
    private readonly IJournalEntryRepository _journalEntryRepository;
    public MenuState(IPanelController panelController, IJournalEntryRepository journalEntryRepository)
    {
        _panelController = panelController;
        _journalEntryRepository = journalEntryRepository;
    }

    public void HandleOption(int optionNumber)
    {
        throw new NotImplementedException();
    }

    public void PrintMenu()
    {
        Console.WriteLine("Choose an action to do in the journal:");
        Console.WriteLine("...WIP");
    }
}