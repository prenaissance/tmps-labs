using Journal.DataAccess.JournalEntries.Abstractions;

namespace Journal.DataAccess.JournalEntries;

public static class JournalRepositoryContext
{
    private static IJournalEntryRepository? _journalEntryRepository;
    public static IJournalEntryRepository JournalEntriesRepository
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