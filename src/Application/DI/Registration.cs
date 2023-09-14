using Journal.Application.DI.Container;
using Journal.Application.JournalEntries.Abstractions;
using Journal.Application.Panels;
using Journal.Application.Panels.Abstractions;
using Journal.Application.Panels.States;
using Journal.Domain.Factory.Abstractions;
using Journal.Domain.Models;

namespace Journal.Application.DI;

public static class Registration
{
    public static DIContainer RegisterApplicationTypes(this DIContainer container)
    {
        PanelController panelController = new();
        var repositoryFactory = container.Resolve<IRepositoryFactory<IJournalEntryRepository, JournalEntry>>();
        panelController.CurrentState = new WelcomeState(panelController, repositoryFactory);
        container.RegisterSingleton<IPanelController>(panelController);

        return container;
    }
}