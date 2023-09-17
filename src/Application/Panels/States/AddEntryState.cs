using Journal.Application.Extensions;
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
        Console.WriteLine();
        Console.CursorVisible = true;

        Console.Write("Title: ");
        string title = Console.ReadLine() ?? "";
        Console.Write("Content: ");
        string content = Console.ReadLine() ?? "";
        Console.CursorVisible = false;

        JournalEntry entry = new(title, content, new List<EntryTag>());
        _journalEntryRepository.Add(entry);
        var entryAddedState = _stateFactory.CreateState<EntryAddedState>(entry);

        string welcomeMessage = $"Entry '{entry.Title}' added".ToColor(ConsoleColor.Green);
        var newState = new ClearConsoleViewDecorator(
            new WelcomeMessageViewDecorator(entryAddedState, welcomeMessage)
        );
        _panelController.ChangeState(newState);
    }
}