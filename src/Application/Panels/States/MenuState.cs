using Journal.Application.JournalEntries;
using Journal.Application.JournalEntries.Abstractions;
using Journal.Application.Panels.Abstractions;
using Journal.Application.Panels.Options;
using Journal.Application.Panels.States.Abstractions;
using Journal.Application.Views;
using Journal.Domain.Models;

namespace Journal.Application.Panels.States;

public class MenuState : IPanelState
{
    private readonly IPanelController _panelController;
    private readonly IJournalEntryRepository _journalEntryRepository;
    private readonly IStateFactory _stateFactory;

    private void HandleAddEntryOption()
    {
        Console.Write("Title: ");
        string title = Console.ReadLine() ?? "";
        Console.Write("Content: ");
        string content = Console.ReadLine() ?? "";

        JournalEntry entry = new(title, content, new List<EntryTag>());
        _journalEntryRepository.Add(entry);
        var entryAddedState = _stateFactory.CreateState<EntryAddedState>();
        entryAddedState.Entry = entry;
        _panelController.ChangeState(entryAddedState);
    }

    private void HandleViewEntriesOption()
    {
        _panelController.ChangeState(_stateFactory.CreateState<ViewEntriesState>());
    }

    private void HandleAddTagOption()
    {
        _panelController.ChangeState(_stateFactory.CreateState<AddTagState<MenuState>>());
    }

    private void HandleSeedEntriesOption()
    {
        throw new NotImplementedException();
    }
    private readonly OptionsHandler _optionsHandler;
    public MenuState(IPanelController panelController, IStateFactory stateFactory)
    {
        _panelController = panelController;
        _journalEntryRepository = JournalRepositoryContext.JournalEntryRepository;
        _optionsHandler = new OptionsBuilder()
            .AddOption("Add entry", HandleAddEntryOption)
            .AddOption("View entries", HandleViewEntriesOption)
            .AddOption("Seed entries", HandleSeedEntriesOption)
            .AddOption("Add tag", HandleAddTagOption)
            .Build();
        _stateFactory = stateFactory;
    }

    public void Render()
    {
        Console.WriteLine("Choose an action to do in the journal:");
        new OptionMenuView(_optionsHandler.Options, _optionsHandler.HandleOption).Render();
    }
}