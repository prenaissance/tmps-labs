namespace Journal.Application.Panels.Options;

public class OptionsBuilder
{
    private readonly IDictionary<int, Action> _options = new Dictionary<int, Action>();
    private readonly IDictionary<int, string> _optionsDescriptions = new Dictionary<int, string>();

    public OptionsBuilder AddOption(int optionNumber, string description, Action action)
    {
        _options.Add(optionNumber, action);
        _optionsDescriptions.Add(optionNumber, description);
        return this;
    }

    public OptionsHandler Build() => new(_options, _optionsDescriptions);
}