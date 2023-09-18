using Journal.Application.JournalEntries.Abstractions;
using Journal.Domain.Models;
using Journal.Domain.Models.Abstractions;

namespace Journal.DataAccess.JournalEntries;

public class MemoryJournalEntryRepository : IJournalEntryRepository
{
    private readonly IList<JournalEntry> _journalEntries;

    public MemoryJournalEntryRepository()
    {
        _journalEntries = new List<JournalEntry>();
    }

    public Task<JournalEntry> Add(JournalEntry journalEntry)
    {
        _journalEntries.Add(journalEntry);
        return Task.FromResult(journalEntry);
    }

    public Task<JournalEntry?> GetById(Guid id)
    {
        return Task.FromResult(_journalEntries.FirstOrDefault(je => je.Id == id));
    }

    public Task<JournalEntry> Delete(Guid id)
    {
        var journalEntry = _journalEntries.FirstOrDefault(je => je.Id == id);
        if (journalEntry is null)
        {
            throw new ArgumentException($"Journal entry with id {id} does not exist");
        }
        _journalEntries.Remove(journalEntry);
        return Task.FromResult(journalEntry);
    }

    public Task<IList<JournalEntry>> GetAll()
    {
        return Task.FromResult(_journalEntries);
    }

    public Task<IList<JournalEntry>> GetManyByTagName(string tag)
    {
        var journalEntries = _journalEntries.Where(
            je => je.Tags
                .Select(t => t.Name)
                .Contains(tag))
            .ToList();
        return Task.FromResult<IList<JournalEntry>>(journalEntries);
    }

    public Task<IList<JournalEntry>> GetManyByTextSearch(string text)
    {
        var journalEntries = _journalEntries.Where(
            je => je.Title.Contains(text) || je.Content.Contains(text))
            .ToList();
        return Task.FromResult<IList<JournalEntry>>(journalEntries);
    }

    public Task<JournalEntry> Update(JournalEntry entity)
    {
        JournalEntry? oldEntry = _journalEntries.FirstOrDefault(je => je.Id == entity.Id);
        if (oldEntry is null)
        {
            throw new ArgumentException($"Journal entry with id {entity.Id} does not exist, tried to update");
        }
        int journalEntryIndex = _journalEntries.IndexOf(oldEntry);
        _journalEntries[journalEntryIndex] = entity;
        return Task.FromResult(entity);
    }

    public Task<JournalEntry> Upsert(JournalEntry entity)
    {
        JournalEntry? oldEntry = _journalEntries.FirstOrDefault(je => je.Id == entity.Id);
        if (oldEntry is null)
        {
            _journalEntries.Add(entity);
        }
        else
        {
            int journalEntryIndex = _journalEntries.IndexOf(oldEntry);
            _journalEntries[journalEntryIndex] = entity;
        }
        return Task.FromResult(entity);
    }
}