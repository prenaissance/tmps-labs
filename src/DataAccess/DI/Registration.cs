using Journal.Application.DI.Container;
using Journal.Application.JournalEntries.Abstractions;
using Journal.Application.Panels.States;
using Journal.DataAccess.JournalEntries;
using Journal.Domain.Factory;
using Journal.Domain.Factory.Abstractions;
using Journal.Domain.Models;

namespace Journal.DataAccess.DI;

public static class Registration
{
    public static DIContainer RegisterDataAccessTypes(this DIContainer container)
    {
        RepositoryFactory<IJournalEntryRepository, JournalEntry> repositoryFactory = new();
        repositoryFactory.Register(
            WelcomeState.Options.MemoryStorage,
            () => new MemoryJournalEntryRepository());
        repositoryFactory.Register(
            WelcomeState.Options.FileStorage,
            () => throw new Exception("File storage not implemented"));

        container.RegisterSingleton<IRepositoryFactory<IJournalEntryRepository, JournalEntry>>(repositoryFactory);
        return container;
    }
}