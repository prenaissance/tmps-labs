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

    public required JournalEntry Entry { get; set; }
    private void HandleViewEntryOption()
    {
        var viewEntryState = _stateFactory.CreateState<ViewEntryState>();
        viewEntryState.Entry = Entry;
        _panelController.ChangeState(viewEntryState);
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
        IStateFactory stateFactory)
    {
        _panelController = panelController;
        _optionsHandler = new OptionsBuilder()
            .AddOption("View entry", HandleViewEntryOption)
            .AddOption("Add tags", HandleAddTagsOption)
            .AddOption("Return to menu", HandleReturnToMenuOption)
            .Build();
        _stateFactory = stateFactory;
    }

    public void Render()
    {
        Console.WriteLine($"Entry '{Entry.Title}' added");
        new OptionMenuView(_optionsHandler.Options, _optionsHandler.HandleOption).Render();
    }
}