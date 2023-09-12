namespace Journal.Application.Panels.Options;

public class OptionsHandler
{
    private readonly IDictionary<int, Action> _options;
    private readonly IDictionary<int, string> _optionsDescriptions;

    public OptionsHandler(IDictionary<int, Action> options, IDictionary<int, string> optionsDescriptions)
    {
        _options = options;
        _optionsDescriptions = optionsDescriptions;
    }
    public IList<int> Options => _options.Keys.ToList();
    public string OptionsMenu => string.Join("\n", _optionsDescriptions.Select(x => $"{x.Key}. {x.Value}"));

    public void HandleOption(int optionNumber)
    {
        if (_options.ContainsKey(optionNumber))
        {
            _options[optionNumber]();
        }
        else
        {
            throw new KeyNotFoundException($"Option number {optionNumber} not registered");
        }
    }
}