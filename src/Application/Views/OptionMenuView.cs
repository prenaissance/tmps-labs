using System.Collections.ObjectModel;
using Journal.Application.Abstractions;
using Journal.Application.Views.Utilities;
namespace Journal.Application.Views;

public class OptionMenuView : IView
{
    private int _currentIndex = 0;
    private readonly IList<string> _options;
    private readonly Action<string> _onOptionSelected;
    private void HandleKeyUp()
    {
        if (_currentIndex == 0)
        {
            _currentIndex = _options.Count - 1;
        }
        else
        {
            _currentIndex--;
        }
        Render();
    }
    private void HandleKeyDown()
    {
        if (_currentIndex == _options.Count - 1)
        {
            _currentIndex = 0;
        }
        else
        {
            _currentIndex++;
        }
        Render();
    }
    private void HandleEnter()
    {
        _onOptionSelected(_options[_currentIndex]);
    }
    private readonly ReadOnlyDictionary<ConsoleKey, Action> _keyHandlers;

    public OptionMenuView(IList<string> options, Action<string> onOptionSelected)
    {
        _options = options;
        _onOptionSelected = onOptionSelected;
        _keyHandlers = new ReadOnlyDictionary<ConsoleKey, Action>(
            new Dictionary<ConsoleKey, Action>
            {
                { ConsoleKey.UpArrow, HandleKeyUp },
                { ConsoleKey.DownArrow, HandleKeyDown },
                { ConsoleKey.Enter, HandleEnter }
            }
        );
    }

    private void ListenForInput()
    {
        ConsoleKey key = Console.ReadKey(true).Key;
        if (_keyHandlers.ContainsKey(key))
        {
            ConsoleUtilities.PreRender(_options.Count);
            _keyHandlers[key]();
        }
        else
        {
            ListenForInput();
        }
    }

    public void Render()
    {
        for (int i = 0; i < _options.Count; i++)
        {
            if (i == _currentIndex)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("> ");
            }
            else
            {
                Console.Write("  ");
            }
            Console.WriteLine(_options[i]);
            Console.ResetColor();
        }
        ListenForInput();
    }
}