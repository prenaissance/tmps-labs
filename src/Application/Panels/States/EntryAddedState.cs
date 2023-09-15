using Journal.Application.Panels.Abstractions;
using Journal.Application.Panels.Options;
using Journal.Application.Panels.States.Abstractions;
using Journal.Application.Views;
using Journal.Domain.Models;

namespace Journal.Application.Panels.States;

public class EntryAddedState : IPanelState
{
    private readonly IPanelController _panelController;
    private readonly IStateFactory _stateFactory;

    private readonly JournalEntry _entry;
    private void HandleViewEntryOption()
    {
        throw new NotImplementedException();
    }
    private void HandleAddTagsOption()
    {
        throw new NotImplementedException();
    }
    private void HandleReturnToMenuOption()
    {
        _panelController.ChangeState(_stateFactory.CreateState<MenuState>());
    }
    private readonly OptionsHandler _optionsHandler;
    public EntryAddedState(
        IPanelController panelController,
        JournalEntry entry,
        IStateFactory stateFactory)
    {
        _panelController = panelController;
        _entry = entry;
        _optionsHandler = new OptionsBuilder()
            .AddOption("View entry", HandleViewEntryOption)
            .AddOption("Add tags", HandleAddTagsOption)
            .AddOption("Return to menu", HandleReturnToMenuOption)
            .Build();
        _stateFactory = stateFactory;
    }

    public void Render()
    {
        Console.WriteLine($"Entry '{_entry.Title}' added");
        new OptionMenuView(_optionsHandler.Options, _optionsHandler.HandleOption).Render();
    }
}