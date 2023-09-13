namespace Journal.Application.Panels.Options;

public class OptionsHandler
{
    private readonly IDictionary<string, Action> _options;

    public OptionsHandler(IDictionary<string, Action> options)
    {
        _options = options;
    }
    public IList<string> Options => _options.Keys.ToList();

    public void HandleOption(string option)
    {
        if (_options.ContainsKey(option))
        {
            _options[option]();
        }
        else
        {
            throw new KeyNotFoundException($"Option '{option}' not registered");
        }
    }
}