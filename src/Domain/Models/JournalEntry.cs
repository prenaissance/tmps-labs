using System.Runtime.Serialization;
using Journal.Domain.Models.Abstractions;

namespace Journal.Domain.Models;

public record JournalEntry(
    string Title,
    string Content,
    IList<EntryTag> Tags
) : Entity, ICopyable<JournalEntry>, ISerializable
{
    public DateTime CreatedAt { get; init; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public JournalEntry Copy() => this with
    {
        Title = $"{Title} (Copy)",
        Tags = new List<EntryTag>(Tags),
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now
    };

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(Id), Id);
        info.AddValue(nameof(Title), Title);
        info.AddValue(nameof(Content), Content);
        info.AddValue(nameof(Tags), Tags);
        info.AddValue(nameof(CreatedAt), CreatedAt);
        info.AddValue(nameof(UpdatedAt), UpdatedAt);
    }
};