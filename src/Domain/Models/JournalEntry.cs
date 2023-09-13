using Journal.Domain.Models.Abstractions;

namespace Journal.Domain.Models;

public record JournalEntry(
    string Title,
    string Content,
    IList<EntryTag> Tags
) : Entity, ICopyable<JournalEntry>
{
    public DateTime CreatedAt { get; init; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public JournalEntry Copy() => this with
    {
        Title = $"{Title} (Copy)",
        Tags = new List<EntryTag>(Tags)
    };
};