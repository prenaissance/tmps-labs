using Journal.Application.Extensions;
using Journal.Application.JournalEntries;
using Journal.Application.JournalEntries.Abstractions;
using Journal.Application.Panels.Abstractions;
using Journal.Application.Panels.Options;
using Journal.Application.Panels.States.Abstractions;
using Journal.Application.Views;
using Journal.Application.Views.Decorators;
using Journal.Domain.Models;

namespace Journal.Application.Panels.States;

public class MenuState : IPanelState
{
    private readonly IPanelController _panelController;
    private readonly JournalEntriesSeeding _journalEntriesSeeding;
    private readonly IStateFactory _stateFactory;

    private void HandleAddEntryOption()
    {
        var newState = new ClearConsoleViewDecorator(_stateFactory.CreateState<AddEntryState>());
        _panelController.ChangeState(newState);
    }

    private void HandleViewEntriesOption()
    {
        var newState = new ClearConsoleViewDecorator(_stateFactory.CreateState<ViewEntriesState>());
        _panelController.ChangeState(newState);
    }

    private void HandleAddTagOption()
    {
        var newState = new ClearConsoleViewDecorator(_stateFactory.CreateState<CreateTagState<MenuState>>());
        _panelController.ChangeState(newState);
    }

    private void HandleSeedEntriesOption()
    {
        _journalEntriesSeeding.SeedEntriesAsync().Wait();

        string message = "Entries seeded successfully".ToColor(ConsoleColor.Green);
        var newState = new ClearConsoleViewDecorator(
            new WelcomeMessageViewDecorator(_stateFactory.CreateState<MenuState>(), message)
        );
        _panelController.ChangeState(newState);
    }
    private readonly OptionsHandler _optionsHandler;
    public MenuState(IPanelController panelController, IStateFactory stateFactory, JournalEntriesSeeding journalEntriesSeeding)
    {
        _panelController = panelController;
        _stateFactory = stateFactory;
        _journalEntriesSeeding = journalEntriesSeeding;
        _optionsHandler = new OptionsBuilder()
            .AddOption("Add entry", HandleAddEntryOption)
            .AddOption("View entries", HandleViewEntriesOption)
            .AddOption("Seed entries", HandleSeedEntriesOption)
            .AddOption("Create tag", HandleAddTagOption)
            .Build();
    }

    public void Render()
    {
        Console.WriteLine("Choose an action to do in the journal:");
        new OptionMenuView(_optionsHandler.Options, _optionsHandler.HandleOption).Render();
    }
}