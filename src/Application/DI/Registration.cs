using Journal.Application.DI.Container;
using Journal.Application.Panels;
using Journal.Application.Panels.Abstractions;
using Journal.Application.Panels.States;

namespace Journal.Application.DI;

public static class Registration
{
    public static DIContainer RegisterApplicationTypes(this DIContainer container)
    {
        PanelController panelController = new();
        panelController.CurrentState = new WelcomeState(panelController);
        container.RegisterSingleton<IPanelController>(panelController);

        return container;
    }
}