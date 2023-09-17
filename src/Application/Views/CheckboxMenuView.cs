using System.Collections.Generic;
using System.Collections.ObjectModel;
using Journal.Application.Abstractions;
using Journal.Application.Extensions;
using Journal.Application.Views.Utilities;
namespace Journal.Application.Views;

public class CheckboxMenuView : IView
{
    private int _currentIndex = 0;
    private readonly IList<string> _checkboxes;
    private readonly HashSet<string> _selectedCheckboxes;
    private readonly Action<string[]> _onComplete;
    private void HandleKeyUp()
    {
        if (_currentIndex == 0)
        {
            _currentIndex = _checkboxes.Count - 1;
        }
        else
        {
            _currentIndex--;
        }
        Render();
    }
    private void HandleKeyDown()
    {
        if (_currentIndex == _checkboxes.Count - 1)
        {
            _currentIndex = 0;
        }
        else
        {
            _currentIndex++;
        }
        Render();
    }
    private void HandleKeyRight()
    {
        if (_selectedCheckboxes.Contains(_checkboxes[_currentIndex]))
        {
            _selectedCheckboxes.Remove(_checkboxes[_currentIndex]);
        }
        else
        {
            _selectedCheckboxes.Add(_checkboxes[_currentIndex]);
        }
        Render();
    }
    private void HandleEnter()
    {
        _onComplete(_selectedCheckboxes.ToArray());
    }
    private readonly ReadOnlyDictionary<ConsoleKey, Action> _keyHandlers;

    public CheckboxMenuView(IList<string> checkboxes, IList<string> selectedCheckboxes, Action<string[]> onOptionSelected)
    {
        _checkboxes = checkboxes;
        _selectedCheckboxes = new HashSet<string>(selectedCheckboxes);
        _onComplete = onOptionSelected;
        _keyHandlers = new ReadOnlyDictionary<ConsoleKey, Action>(
            new Dictionary<ConsoleKey, Action>
            {
                { ConsoleKey.UpArrow, HandleKeyUp },
                { ConsoleKey.DownArrow, HandleKeyDown },
                { ConsoleKey.RightArrow, HandleKeyRight },
                { ConsoleKey.Enter, HandleEnter }
            }
        );
    }

    private void ListenForInput()
    {
        ConsoleKey key = Console.ReadKey(true).Key;
        if (_keyHandlers.ContainsKey(key))
        {
            ConsoleUtilities.PreRender(_checkboxes.Count + 2);
            _keyHandlers[key]();
        }
        else
        {
            ListenForInput();
        }
    }

    public void Render()
    {
        Console.WriteLine("↑/↓ to navigate, → to toggle selection and ↩ to confirm.");
        Console.WriteLine();
        for (int i = 0; i < _checkboxes.Count; i++)
        {
            if (i == _currentIndex)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
            }
            if (_selectedCheckboxes.Contains(_checkboxes[i]))
            {
                Console.Write($"[{"x".ToColor(ConsoleColor.Green)}] ");
            }
            else
            {
                Console.Write("[ ] ");
            }
            Console.WriteLine(_checkboxes[i]);
            Console.ResetColor();
        }
        ListenForInput();
    }
}