using Journal.Domain.Models.Abstractions;

namespace Journal.Domain.Models;

public record JournalEntry(
    string Title,
    string Content,
    IList<EntryTag> Tags,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    Guid UserId
) : Entity, ICopyable<JournalEntry>
{
    public JournalEntry Copy() => this with
    {
        Title = $"{Title} (Copy)",
        Tags = new List<EntryTag>(Tags)
    };
};