using Journal.Application.JournalEntries.Abstractions;

namespace Journal.Application.JournalEntries;

public static class JournalRepositoryContext
{
    private static IJournalEntryRepository? _journalEntryRepository;
    public static IJournalEntryRepository JournalEntryRepository
    {
        get
        {
            if (_journalEntryRepository is null)
            {
                throw new InvalidOperationException("Journal entry repository not initialized");
            }
            return _journalEntryRepository;
        }
        set => _journalEntryRepository = value;
    }
}