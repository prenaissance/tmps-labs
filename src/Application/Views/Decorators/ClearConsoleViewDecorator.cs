using Journal.Application.Abstractions;

namespace Journal.Application.Views.Decorators;

public class ClearConsoleViewDecorator : IView
{
    private readonly IView _view;
    public ClearConsoleViewDecorator(IView view)
    {
        _view = view;
    }
    public void Render()
    {
        Console.Clear();
        _view.Render();
    }
}