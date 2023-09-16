using Journal.Application.JournalEntries;
using Journal.Application.JournalEntries.Abstractions;
using Journal.Application.Panels.Abstractions;
using Journal.Application.Panels.Options;
using Journal.Application.Panels.States.Abstractions;
using Journal.Application.Views;
using Journal.Application.Views.Decorators;
using Journal.Domain.Factory.Abstractions;
using Journal.Domain.Models;

namespace Journal.Application.Panels.States;

public class WelcomeState : IPanelState
{
    public static class Options
    {
        public const string MemoryStorage = "In memory";
        public const string FileStorage = "In file storage (WIP)";
    }
    private readonly IPanelController _panelController;
    private readonly IRepositoryFactory<IJournalEntryRepository, JournalEntry> _journalEntryRepositoryFactory;
    private readonly IStateFactory _stateFactory;
    private void HandleMemoryStorageOption()
    {
        JournalRepositoryContext.JournalEntryRepository = _journalEntryRepositoryFactory.CreateRepository(Options.MemoryStorage);
        var menuState = _stateFactory.CreateState<MenuState>();
        _panelController.ChangeState(
            new ClearConsoleViewDecorator(menuState)
        );
    }
    private void HandleFileStorageOption()
    {
        throw new NotImplementedException();
    }
    private readonly OptionsHandler _optionsHandler;
    public WelcomeState(
        IPanelController panelController,
        IRepositoryFactory<IJournalEntryRepository, JournalEntry> journalEntryRepositoryFactory,
        IStateFactory stateFactory)
    {
        _panelController = panelController;
        _journalEntryRepositoryFactory = journalEntryRepositoryFactory;
        _optionsHandler = new OptionsBuilder()
            .AddOption(Options.MemoryStorage, HandleMemoryStorageOption)
            .AddOption(Options.FileStorage, HandleFileStorageOption)
            .Build();
        _stateFactory = stateFactory;
    }

    public void Render()
    {
        Console.WriteLine("Welcome to the Journal Application!");
        Console.WriteLine("You can make journal entries, tag them and some other operations.");
        Console.WriteLine("Please select where you want to store your journal entries:");
        Console.WriteLine();
        new OptionMenuView(_optionsHandler.Options, _optionsHandler.HandleOption).Render();
    }
}