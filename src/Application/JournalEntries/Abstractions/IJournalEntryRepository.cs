namespace Journal.Domain.Models.Abstractions;

public interface IJournalEntryRepository : IRepository<JournalEntry>
{
    Task<IList<JournalEntry>> GetByTagName(string tag);
}