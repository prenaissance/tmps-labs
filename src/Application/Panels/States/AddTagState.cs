using Journal.Application.Common.Utilities;
using Journal.Application.Extensions;
using Journal.Application.Panels.Abstractions;
using Journal.Application.Panels.States.Abstractions;
using Journal.Application.Tags.Factory;
using Journal.Application.Views;
using Journal.Application.Views.Decorators;

namespace Journal.Application.Panels.States;

public class AddTagState<NextStateT> : IPanelState where NextStateT : IPanelState
{
    private readonly IPanelController _panelController;
    private readonly IStateFactory _stateFactory;
    private readonly ITagFactory _tagFactory;
    private string _name = "";
    private void HandleSelectTagColor(string color)
    {
        ConsoleColor consoleColor;
        Enum.TryParse(color, out consoleColor);
        consoleColor = consoleColor == default ? ConsoleColor.White : consoleColor;
        _tagFactory.AddTag(_name, consoleColor);

        IPanelState nextState = _stateFactory.CreateState<NextStateT>();
        string welcomeMessage = $"Tag '{_name}' added".ToColor(ConsoleColor.Green);
        var welcomeMessageNextState = new WelcomeMessageViewDecorator(nextState, welcomeMessage);
        _panelController.ChangeState(welcomeMessageNextState);
    }
    public AddTagState(IPanelController panelController, IStateFactory stateFactory, ITagFactory tagFactory)
    {
        _panelController = panelController;
        _stateFactory = stateFactory;
        _tagFactory = tagFactory;
    }

    public void Render()
    {
        Console.Write("Enter tag name: ");
        _name = Console.ReadLine() ?? "";
        Console.WriteLine("Select tag color:");
        new OptionMenuView(AnsiColors.ansiColors, HandleSelectTagColor).Render();
    }
}