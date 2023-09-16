using Journal.Domain.Models;

namespace Journal.Application.Tags.Factory;

public interface ITagFactory
{
    IList<EntryTag> Instances { get; }
    EntryTag GetTag(string name, ConsoleColor consoleColor);
    void AddTag(string name, ConsoleColor consoleColor);
}