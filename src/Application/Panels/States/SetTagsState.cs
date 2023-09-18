using Journal.Application.Extensions;
using Journal.Application.JournalEntries;
using Journal.Application.JournalEntries.Abstractions;
using Journal.Application.Panels.Abstractions;
using Journal.Application.Panels.Options;
using Journal.Application.Panels.States.Abstractions;
using Journal.Application.Tags.Factory;
using Journal.Application.Tags.Utilities;
using Journal.Application.Views;
using Journal.Application.Views.Decorators;
using Journal.Domain.Models;

namespace Journal.Application.Panels.States;

public class SetTagsState<NextStateT> : IPanelState where NextStateT : IPanelState
{
    private readonly IPanelController _panelController;
    private readonly IStateFactory _stateFactory;
    private readonly ITagFactory _tagFactory;
    private readonly IJournalEntryRepository _journalEntryRepository;
    private readonly JournalEntry _entry;
    private void HandleTagsSelected(string[] tags)
    {
        var coloredTagsDictionary = _tagFactory.Instances.ToDictionary(TagUtilities.GetTagString, tag => tag);
        JournalEntry newEntry = _entry with
        {
            Tags = tags.Select(tag => coloredTagsDictionary[tag]).ToList()
        };
        _journalEntryRepository.Update(newEntry);

        IPanelState nextState = _stateFactory.CreateState<NextStateT>(newEntry);
        string welcomeMessage = $"Tags for entry '{_entry.Title}' edited".ToColor(ConsoleColor.Green);
        var newState = new ClearConsoleViewDecorator(
            new WelcomeMessageViewDecorator(nextState, welcomeMessage)
        );
        _panelController.ChangeState(newState);
    }
    public SetTagsState(
        IPanelController panelController,
        IStateFactory stateFactory,
        ITagFactory tagFactory,
        JournalEntry entry)
    {
        _panelController = panelController;
        _stateFactory = stateFactory;
        _tagFactory = tagFactory;
        _entry = entry;
        _journalEntryRepository = JournalRepositoryContext.JournalEntryRepository;
    }

    public void Render()
    {
        Console.WriteLine($"Entry '{_entry.Title}' tags:");

        string[] coloredTags = _tagFactory.Instances.Select(TagUtilities.GetTagString).ToArray();
        string[] selectedColoredTags = _entry.Tags.Select(TagUtilities.GetTagString).ToArray();

        new CheckboxMenuView(coloredTags, selectedColoredTags, HandleTagsSelected).Render();
    }
}