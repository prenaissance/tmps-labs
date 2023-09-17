using Journal.Application.Panels.Abstractions;
using Journal.Application.Panels.Options;
using Journal.Application.Panels.States.Abstractions;
using Journal.Application.Views;
using Journal.Application.Views.Decorators;
using Journal.Domain.Models;

namespace Journal.Application.Panels.States;

public class EntryAddedState : IPanelState
{
    private readonly IPanelController _panelController;
    private readonly IStateFactory _stateFactory;
    private readonly JournalEntry _entry;
    private void HandleViewEntryOption()
    {
        var viewEntryState = _stateFactory.CreateState<ViewEntryState>(_entry);
        _panelController.ChangeState(new ClearConsoleViewDecorator(viewEntryState));
    }
    private void HandleSetTagsOption()
    {
        var setTagsState = _stateFactory.CreateState<SetTagsState<EntryAddedState>>(_entry);
        _panelController.ChangeState(new ClearConsoleViewDecorator(setTagsState));
    }
    private void HandleReturnToMenuOption()
    {
        _panelController.ChangeState(new ClearConsoleViewDecorator(_stateFactory.CreateState<MenuState>()));
    }
    private readonly OptionsHandler _optionsHandler;
    public EntryAddedState(
        IPanelController panelController,
        IStateFactory stateFactory,
        JournalEntry entry)
    {
        _panelController = panelController;
        _stateFactory = stateFactory;
        _entry = entry;
        _optionsHandler = new OptionsBuilder()
            .AddOption("View entry", HandleViewEntryOption)
            .AddOption("Add tags", HandleSetTagsOption)
            .AddOption("Return to menu", HandleReturnToMenuOption)
            .Build();
    }

    public void Render()
    {
        new OptionMenuView(_optionsHandler.Options, _optionsHandler.HandleOption).Render();
    }
}