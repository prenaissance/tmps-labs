using System.Collections.ObjectModel;

namespace Journal.Application.Panels.Options;

public class OptionsBuilder
{
    private readonly IDictionary<string, Action> _options = new Dictionary<string, Action>();

    public OptionsBuilder AddOption(string description, Action action)
    {
        _options.Add(description, action);
        return this;
    }

    public OptionsHandler Build() => new(new ReadOnlyDictionary<string, Action>(_options));
}