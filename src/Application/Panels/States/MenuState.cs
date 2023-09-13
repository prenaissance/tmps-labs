using Journal.Application.Panels.Abstractions;
using Journal.Application.Panels.States.Abstractions;
using Journal.DataAccess.JournalEntries;
using Journal.DataAccess.JournalEntries.Abstractions;

namespace Journal.Application.Panels.States;

public class MenuState : IPanelState
{
    private readonly IPanelController _panelController;
    private readonly IJournalEntryRepository _journalEntryRepository;
    public MenuState(IPanelController panelController)
    {
        _panelController = panelController;
        _journalEntryRepository = JournalRepositoryContext.JournalEntriesRepository;
    }

    public void HandleOption(string option)
    {
        throw new NotImplementedException();
    }

    public void Render()
    {
        Console.WriteLine("Choose an action to do in the journal:");
        Console.WriteLine("...WIP");
    }
}