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

    private void HandleAddEntryOption()
    {
        Console.Write("Title: ");
        string title = Console.ReadLine() ?? "";
        Console.Write("Content: ");
        string content = Console.ReadLine() ?? "";
        JournalEntry entry = new(title, content, new List<EntryTag>());
        _journalEntryRepository.Add(entry);
        _panelController.ChangeState(new EntryAddedState(_panelController, entry));
    }
    private readonly OptionsHandler _optionsHandler;
    public MenuState(IPanelController panelController)
    {
        _panelController = panelController;
        _journalEntryRepository = JournalRepositoryContext.JournalEntryRepository;
        _optionsHandler = new OptionsBuilder()
            .AddOption("Add entry", HandleAddEntryOption)
            .Build();
    }

    public void Render()
    {
        Console.WriteLine("Choose an action to do in the journal:");
        new OptionMenuView(_optionsHandler.Options, _optionsHandler.HandleOption).Render();
    }
}