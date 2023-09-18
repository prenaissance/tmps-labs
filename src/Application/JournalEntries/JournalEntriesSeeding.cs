using Journal.Application.JournalEntries.Abstractions;
using Journal.Application.Tags.Factory;
using Journal.Domain.Models;

namespace Journal.Application.JournalEntries;

public class JournalEntriesSeeding
{
    private readonly IJournalEntryRepository _journalEntryRepository;
    private readonly ITagFactory _tagFactory;

    public JournalEntriesSeeding(ITagFactory tagFactory)
    {
        _tagFactory = tagFactory;
        _journalEntryRepository = JournalRepositoryContext.JournalEntryRepository;
    }

    public async Task SeedEntriesAsync()
    {
        var seededEntries = new[]
        {
            new JournalEntry(
                "First entry",
                "This is the first entry",
                new List<EntryTag> {
                    _tagFactory.GetTag("Random Tag", ConsoleColor.DarkBlue)
                }
            ) {
                Id = Guid.Parse("11111111-4c1a-4b0f-9f6a-5a4d1b4b1b1b")
            },
            new JournalEntry(
                "Homework",
                "I need to do labs for TMPS and CS",
                new List<EntryTag> {
                    _tagFactory.GetTag("Homework", ConsoleColor.DarkRed),
                    _tagFactory.GetTag("Important", ConsoleColor.Red),
                }
            ) {
                Id = Guid.Parse("22222222-4c1a-4b0f-9f6a-5a4d1b4b1b1b")
            },
            new JournalEntry(
                "Death Note",
                "John Doe, 20 years old, died of a heart attack",
                new List<EntryTag> {
                    _tagFactory.GetTag("Random Tag", ConsoleColor.DarkBlue)
                }
            ) {
                Id = Guid.Parse("33333333-4c1a-4b0f-9f6a-5a4d1b4b1b1b")
            },
        };

        foreach (var entry in seededEntries)
        {
            await _journalEntryRepository.Upsert(entry);
        }
    }
}