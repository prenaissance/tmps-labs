using System.Text.Json;
using Journal.Application.JournalEntries.Abstractions;
using Journal.Application.Tags.Factory;
using Journal.Domain.Models;

namespace Journal.DataAccess.JournalEntries;

public class FileJournalEntryRepository : IJournalEntryRepository
{
    private const string FILE_NAME = "journal_entries.json";
    private readonly ITagFactory _tagFactory;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    private static FileStream GetFileStream() => File.Open(FILE_NAME, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    private async Task<(List<JournalEntry> journalEntries, FileStream fileStream)> GetJournalEntries()
    {
        var fileStream = GetFileStream();
        try
        {
            return (await JsonSerializer.DeserializeAsync<List<JournalEntry>>(fileStream, _jsonSerializerOptions) ?? new List<JournalEntry>(), fileStream);
        }
        catch (JsonException)
        {
            await JsonSerializer.SerializeAsync(fileStream, new List<JournalEntry>(), _jsonSerializerOptions);
            return (new List<JournalEntry>(), fileStream);
        }
    }
    private async Task SerializeEntriesAsync(List<JournalEntry> journalEntries, FileStream fileStream)
    {
        fileStream.Seek(0, SeekOrigin.Begin);
        await JsonSerializer.SerializeAsync(fileStream, journalEntries, _jsonSerializerOptions);
        await fileStream.DisposeAsync();
    }
    public FileJournalEntryRepository(ITagFactory tagFactory)
    {
        _tagFactory = tagFactory;
        var (journalEntries, fileStream) = GetJournalEntries().Result;
        journalEntries.SelectMany(je => je.Tags).ToList().ForEach(t => _tagFactory.AddTag(t.Name, t.Color));
        fileStream.Close();
    }

    public async Task<JournalEntry> Add(JournalEntry entity)
    {
        var (journalEntries, fileStream) = await GetJournalEntries();
        journalEntries.Add(entity);
        try
        {
            fileStream.Seek(0, SeekOrigin.Begin);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        await SerializeEntriesAsync(journalEntries, fileStream);
        return entity;
    }

    public async Task<JournalEntry> Delete(Guid id)
    {
        var (journalEntries, fileStream) = await GetJournalEntries();
        var journalEntry = journalEntries.FirstOrDefault(je => je.Id == id);
        if (journalEntry is null)
        {
            throw new ArgumentException($"Journal entry with id {id} does not exist");
        }
        journalEntries.Remove(journalEntry);
        await SerializeEntriesAsync(journalEntries, fileStream);
        return journalEntry;
    }

    public async Task<IList<JournalEntry>> GetAll()
    {
        var (journalEntries, fileStream) = await GetJournalEntries();
        await fileStream.DisposeAsync();
        return journalEntries;
    }

    public async Task<JournalEntry?> GetById(Guid id)
    {
        var (journalEntries, fileStream) = await GetJournalEntries();
        await fileStream.DisposeAsync();
        return journalEntries.FirstOrDefault(je => je.Id == id);
    }

    public async Task<IList<JournalEntry>> GetManyByTagName(string tag)
    {
        var (journalEntries, fileStream) = await GetJournalEntries();
        await fileStream.DisposeAsync();
        var journalEntriesWithTag = journalEntries.Where(
            je => je.Tags
                .Select(t => t.Name)
                .Contains(tag))
            .ToList();
        return journalEntriesWithTag;
    }

    public async Task<IList<JournalEntry>> GetManyByTextSearch(string text)
    {
        var (journalEntries, fileStream) = await GetJournalEntries();
        await fileStream.DisposeAsync();
        var journalEntriesWithText = journalEntries.Where(
            je => je.Title.Contains(text) || je.Content.Contains(text))
            .ToList();
        return journalEntriesWithText;
    }

    public async Task<JournalEntry> Update(JournalEntry entity)
    {
        var (journalEntries, fileStream) = await GetJournalEntries();
        JournalEntry? oldEntry = journalEntries.FirstOrDefault(je => je.Id == entity.Id)
            ?? throw new ArgumentException($"Journal entry with id {entity.Id} does not exist");

        int journalEntryIndex = journalEntries.IndexOf(oldEntry);
        journalEntries[journalEntryIndex] = entity;
        await SerializeEntriesAsync(journalEntries, fileStream);
        return entity;
    }

    public async Task<JournalEntry> Upsert(JournalEntry entity)
    {
        JournalEntry? oldEntry = await GetById(entity.Id);
        if (oldEntry is null)
        {
            return await Add(entity);
        }
        return await Update(entity);
    }
}