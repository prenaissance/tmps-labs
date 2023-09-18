using Journal.Application.Extensions;
using Journal.Application.JournalEntries;
using Journal.Application.JournalEntries.Abstractions;
using Journal.Application.Panels.Abstractions;
using Journal.Application.Panels.Options;
using Journal.Application.Panels.States.Abstractions;
using Journal.Application.Tags.Utilities;
using Journal.Application.Views;
using Journal.Application.Views.Decorators;
using Journal.Domain.Models;

namespace Journal.Application.Panels.States;

public class ViewEntryState : IPanelState
{
    private readonly IPanelController _panelController;
    private readonly IStateFactory _stateFactory;
    private readonly IJournalEntryRepository _journalEntryRepository;
    private readonly OptionsHandler _optionsHandler;
    private readonly JournalEntry _entry;
    private void HandleEditEntryOption()
    {
        string message = "I changed my mind! I'm not going to implement this!".ToColor(ConsoleColor.Red);

        var newState = new ClearConsoleViewDecorator(
            new WelcomeMessageViewDecorator(_stateFactory.CreateState<ViewEntryState>(_entry), message)
        );
        _panelController.ChangeState(newState);
    }
    private void HandleDeleteEntryOption()
    {
        _journalEntryRepository.Delete(_entry.Id).Wait();
        string message = $"Entry {_entry.Title} deleted successfully".ToColor(ConsoleColor.Green);
        var newState = new ClearConsoleViewDecorator(
            new WelcomeMessageViewDecorator(_stateFactory.CreateState<ViewEntriesState>(), message)
        );
        _panelController.ChangeState(newState);
    }
    private void CopyEntryOption()
    {
        JournalEntry copiedEntry = _entry.Copy();
        _journalEntryRepository.Add(copiedEntry).Wait();

        string message = $"Entry {_entry.Title} copied successfully".ToColor(ConsoleColor.Green);
        var newState = new ClearConsoleViewDecorator(
            new WelcomeMessageViewDecorator(_stateFactory.CreateState<EntryAddedState>(copiedEntry), message)
        );
        _panelController.ChangeState(newState);
    }
    private void EditTagsOption()
    {
        var setTagsState = _stateFactory.CreateState<SetTagsState<ViewEntryState>>(_entry);
        _panelController.ChangeState(new ClearConsoleViewDecorator(setTagsState));
    }
    private void HandleReturnToMenuOption()
    {
        _panelController.ChangeState(new ClearConsoleViewDecorator(_stateFactory.CreateState<MenuState>()));
    }
    public ViewEntryState(IPanelController panelController, IStateFactory stateFactory, JournalEntry entry)
    {
        _panelController = panelController;
        _stateFactory = stateFactory;
        _entry = entry;
        _journalEntryRepository = JournalRepositoryContext.JournalEntryRepository;
        _optionsHandler = new OptionsBuilder()
            .AddOption("Edit entry", HandleEditEntryOption)
            .AddOption("Delete entry", HandleDeleteEntryOption)
            .AddOption("Copy entry", CopyEntryOption)
            .AddOption("Edit tags", EditTagsOption)
            .AddOption("Return to menu", HandleReturnToMenuOption)
            .Build();
    }

    public void Render()
    {
        Console.WriteLine($"Title: {_entry.Title.ToColor(ConsoleColor.Gray)}");
        Console.WriteLine($"Tags: {TagUtilities.GetFormattedTags(_entry.Tags)}");
        Console.WriteLine("------------------------");
        Console.WriteLine(_entry.Content);
        new OptionMenuView(_optionsHandler.Options, _optionsHandler.HandleOption).Render();
    }
}