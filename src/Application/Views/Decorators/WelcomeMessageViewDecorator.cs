using Journal.Application.Abstractions;
using Journal.Application.Panels.States.Abstractions;

namespace Journal.Application.Views.Decorators;

public class WelcomeMessageViewDecorator : IPanelState
{
    private readonly IView _view;
    private readonly string _message;

    public WelcomeMessageViewDecorator(IView view, string message)
    {
        _view = view;
        _message = message;
    }

    public void Render()
    {
        Console.WriteLine(_message);
        Console.WriteLine();
        _view.Render();
    }
}