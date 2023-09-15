using Journal.Application.Extensions;
using Journal.Application.Panels.Abstractions;
using Journal.Application.Panels.Options;
using Journal.Application.Panels.States.Abstractions;
using Journal.Application.Views;
using Journal.Domain.Models;

namespace Journal.Application.Panels.States;

public class ViewEntryState : IPanelState
{
    private readonly IPanelController _panelController;
    private readonly IStateFactory _stateFactory;
    private readonly OptionsHandler _optionsHandler;
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
        _panelController.ChangeState(_stateFactory.CreateState<MenuState>());
    }
    public ViewEntryState(IPanelController panelController, IStateFactory stateFactory)
    {
        _panelController = panelController;
        _stateFactory = stateFactory;
        _optionsHandler = new OptionsBuilder()
            .AddOption("Edit entry", HandleEditEntryOption)
            .AddOption("Delete entry", HandleDeleteEntryOption)
            .AddOption("Copy entry", CopyEntryOption)
            .AddOption("Edit tags", EditTagsOption)
            .AddOption("Return to menu", HandleReturnToMenuOption)
            .Build();
    }
    public required JournalEntry Entry { private get; set; }

    public void Render()
    {
        Console.WriteLine($"Title: {Entry.Title.ToColor(ConsoleColor.Gray)}");
        Console.WriteLine($"Tags: WIP");
        Console.WriteLine("------------------------");
        Console.WriteLine(Entry.Content);
        new OptionMenuView(_optionsHandler.Options, _optionsHandler.HandleOption).Render();
    }
}