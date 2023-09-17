using Journal.Application.Extensions;
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
    private readonly OptionsHandler _optionsHandler;
    private readonly JournalEntry _entry;
    private void HandleEditEntryOption()
    {
        throw new NotImplementedException();
    }
    private void HandleDeleteEntryOption()
    {
        throw new NotImplementedException();
    }
    private void CopyEntryOption()
    {
        throw new NotImplementedException();
    }
    private void EditTagsOption()
    {
        throw new NotImplementedException();
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