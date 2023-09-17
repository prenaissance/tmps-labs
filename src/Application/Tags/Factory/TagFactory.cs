using Journal.Domain.Models;

namespace Journal.Application.Tags.Factory;

public class TagFactory : ITagFactory
{
    // should ideally be a weak dictionary
    private readonly Dictionary<string, EntryTag> _instancesDictionary = new() {
        { $"Important:{ConsoleColor.Red}", new EntryTag("Important", ConsoleColor.Red) }
    };
    public IList<EntryTag> Instances => _instancesDictionary.Values.ToArray();

    public EntryTag GetTag(string name, ConsoleColor consoleColor)
    {
        string hashKey = $"{name}:{consoleColor}";
        if (_instancesDictionary.ContainsKey(hashKey))
        {
            return _instancesDictionary[hashKey];
        }
        EntryTag tag = new(name, consoleColor);
        _instancesDictionary.Add(hashKey, tag);
        return tag;
    }
    public void AddTag(string name, ConsoleColor consoleColor)
    {
        GetTag(name, consoleColor);
    }
}