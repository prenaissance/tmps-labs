using Journal.Application.Extensions;
using Journal.Application.JournalEntries;
using Journal.Application.JournalEntries.Abstractions;
using Journal.Application.Panels.Abstractions;
using Journal.Application.Panels.Options;
using Journal.Application.Panels.States.Abstractions;
using Journal.Application.Views;
using Journal.Domain.Models;

namespace Journal.Application.Panels.States;

public class ViewEntriesState : IPanelState
{
    private readonly IPanelController _panelController;
    private readonly IStateFactory _stateFactory;
    private readonly IJournalEntryRepository _journalEntryRepository;
    private readonly IList<JournalEntry> _entries;
    private Action GetHandleViewEntryOption(JournalEntry entry) => () =>
    {
        var viewEntryState = _stateFactory.CreateState<ViewEntryState>();
        viewEntryState.Entry = entry;
        _panelController.ChangeState(viewEntryState);
    };
    private void HandleReturnToMenuOption()
    {
        _panelController.ChangeState(_stateFactory.CreateState<MenuState>());
    }

    private readonly OptionsHandler _optionsHandler;

    public ViewEntriesState(IPanelController panelController, IStateFactory stateFactory)
    {
        _panelController = panelController;
        _stateFactory = stateFactory;
        _journalEntryRepository = JournalRepositoryContext.JournalEntryRepository;
        _entries = _journalEntryRepository.GetAll().Result;
        OptionsBuilder optionsBuilder = new();
        foreach (JournalEntry entry in _entries)
        {
            optionsBuilder.AddOption(GetEntryString(entry), GetHandleViewEntryOption(entry));
        }
        optionsBuilder.AddOption("Return to menu", HandleReturnToMenuOption);
        _optionsHandler = optionsBuilder.Build();
    }

    private string GetEntryString(JournalEntry entry)
    {
        return $"Title: {entry.Title.ToColor(ConsoleColor.Cyan)}    Tags: WIP";
    }
    public void Render()
    {
        Console.WriteLine("Entries:");
        new OptionMenuView(_optionsHandler.Options, _optionsHandler.HandleOption).Render();
    }
}