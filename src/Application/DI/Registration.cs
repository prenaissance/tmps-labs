using Journal.Application.DI.Container;
using Journal.Application.JournalEntries;
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
            .RegisterSingleton<ITagFactory, TagFactory>()
            .RegisterSingleton<JournalEntriesSeeding>();

        container
            .RegisterTransient<WelcomeState>()
            .RegisterTransient<MenuState>()
            .RegisterTransient<AddEntryState>()
            .RegisterTransient<EntryAddedState>()
            .RegisterTransient<ViewEntryState>()
            .RegisterTransient<ViewEntriesState>()
            .RegisterTransient<CreateTagState<MenuState>>()
            .RegisterTransient<SetTagsState<EntryAddedState>>()
            .RegisterTransient<SetTagsState<ViewEntryState>>();

        panelController.CurrentState = container.Resolve<WelcomeState>();

        return container;
    }
}