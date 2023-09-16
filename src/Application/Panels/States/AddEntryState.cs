using Journal.Application.JournalEntries;
using Journal.Application.JournalEntries.Abstractions;
using Journal.Application.Panels.Abstractions;
using Journal.Application.Panels.States.Abstractions;
using Journal.Application.Views.Decorators;
using Journal.Domain.Models;

namespace Journal.Application.Panels.States;

public class AddEntryState : IPanelState
{
    private readonly IPanelController _panelController;
    private readonly IStateFactory _stateFactory;
    private readonly IJournalEntryRepository _journalEntryRepository;
    public AddEntryState(IPanelController panelController, IStateFactory stateFactory)
    {
        _panelController = panelController;
        _stateFactory = stateFactory;
        _journalEntryRepository = JournalRepositoryContext.JournalEntryRepository;
    }
    public void Render()
    {
        Console.WriteLine("Add Entry");

        Console.Write("Title: ");
        string title = Console.ReadLine() ?? "";
        Console.Write("Content: ");
        string content = Console.ReadLine() ?? "";

        JournalEntry entry = new(title, content, new List<EntryTag>());
        _journalEntryRepository.Add(entry);
        var entryAddedState = _stateFactory.CreateState<EntryAddedState>();
        entryAddedState.Entry = entry;
        var newState = new ClearConsoleViewDecorator(entryAddedState);
        _panelController.ChangeState(newState);
    }
}