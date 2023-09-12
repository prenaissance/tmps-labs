using Journal.Domain.Models;
using Journal.Domain.Models.Abstractions;

namespace Journal.DataAccess.JournalEntries.Abstractions;

public interface IJournalEntryRepository : IRepository<JournalEntry>
{
    Task<IList<JournalEntry>> GetManyByTagName(string tag);
    Task<IList<JournalEntry>> GetManyByTextSearch(string text);
}