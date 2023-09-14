using Journal.Domain.Models;
using Journal.Domain.Models.Abstractions;

namespace Journal.Application.JournalEntries.Abstractions;

public interface IJournalEntryRepository : IRepository<JournalEntry>
{
    Task<IList<JournalEntry>> GetManyByTagName(string tag);
    Task<IList<JournalEntry>> GetManyByTextSearch(string text);
}