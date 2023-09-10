namespace Journal.Domain.Models;

public record JournalEntry(
    Guid Id,
    string Title,
    string Content,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    Guid UserId
);