using Journal.Application.DI.Container;
using Journal.Application.Panels;
using Journal.Application.Panels.Abstractions;
using Journal.Application.Panels.States;
using Journal.Application.Panels.States.Abstractions;
using Journal.Application.Panels.States.Factory;
using Journal.Application.Tags.Factory;

namespace Journal.Application.DI;

public static class Registration
{
    public static DIContainer RegisterApplicationTypes(this DIContainer container)
    {
        PanelController panelController = new PanelController();
        container
            .RegisterSingleton<IPanelController>(panelController)
            .RegisterSingleton<IStateFactory>(new StateFactory(container))
            .RegisterSingleton<ITagFactory, TagFactory>();

        container
            .RegisterTransient<WelcomeState>()
            .RegisterTransient<MenuState>()
            .RegisterTransient<EntryAddedState>()
            .RegisterTransient<ViewEntryState>()
            .RegisterTransient<ViewEntriesState>()
            .RegisterTransient<AddTagState<MenuState>>();

        panelController.CurrentState = container.Resolve<WelcomeState>();

        return container;
    }
}