# Topic: Creational Design Patterns

## Author: Andrieș Alexandru

---

## Objectives:

**1. Study and understand the Creational Design Patterns.**

**2. Choose a domain, define its main classes/models/entities and choose the appropriate instantiation mechanisms.**

**3. Use some creational design patterns for object instantiation in a sample project.**

## Achieved goals:

- Continued developing the previously chosen topic of a Journal Console Application.

- Implemented creational design patterns.

## Implementation

To create the basic functionality needed for laboratory 1, I already used many creational patterns. I have added some functionality and tests since lab 1.

The functionality is far from complete, at the moment the UI menu is complete, the state machine, dependency injection, models and the in memory repository, but some parts are not glued together. You can check the output by following the instructions in the [README](../README.md).

## Creational Patterns Used

### Singleton

Since I had to implement a DI container, registering singletons was a part of it. Note - my implementation is not thread safe, hope no complains appear about that.

```csharp
// src/Application/DI/Container/DIContainer.cs
using Journal.Application.DI.Container.Enums;

namespace Journal.Application.DI.Container;

public class DIContainer
{
    private readonly Dictionary<Type, TypeRegistration> _typeRegistrations = new();
    private readonly Dictionary<Type, object> _singletonInstances = new();
    public DIContainer RegisterSingleton<TInterface, TImplementation>() where TImplementation : TInterface
    {
        _typeRegistrations[typeof(TInterface)] = new TypeRegistration(
            typeof(TInterface),
            typeof(TImplementation),
            InjectionType.Singleton);

        return this;
    }
    public DIContainer RegisterSingleton<TInterface>(TInterface instance) where TInterface : notnull
    {
        _singletonInstances[typeof(TInterface)] = instance;
        _typeRegistrations[typeof(TInterface)] = new TypeRegistration(
            typeof(TInterface),
            typeof(TInterface),
            InjectionType.Singleton);

        return this;
    }
    // ...
    public TInterface Resolve<TInterface>()
    {
        return (TInterface)Resolve(typeof(TInterface));
    }

    private object Resolve(Type type)
    {
        var typeRegistration = _typeRegistrations.GetValueOrDefault(type);
        if (typeRegistration == null)
        {
            throw new InvalidOperationException($"Type {type.Name} is not registered.");
        }

        if (typeRegistration.InjectionType == InjectionType.Singleton)
        {
            if (!_singletonInstances.ContainsKey(type))
            {
                var constructor = typeRegistration.ImplementationType.GetConstructors()[0];
                _singletonInstances[type] = constructor.Invoke(
                    constructor.GetParameters()
                        .Select(parameter => Resolve(parameter.ParameterType))
                        .ToArray());
            }

            return _singletonInstances[type];
        }

        var implementationConstructor = typeRegistration.ImplementationType.GetConstructors()[0];
        return implementationConstructor.Invoke(
            implementationConstructor.GetParameters()
                .Select(parameter => Resolve(parameter.ParameterType))
                .ToArray());
    }
}
```

Here's a test:

```csharp
// src/Application.Test/DI/DiContainer.Test.cs
[TestMethod]
public void Should_Register_Singleton_With_Instance()
{
    var list = new List<string>() { "test" };
    container.RegisterSingleton<IList<string>>(list);
    var resolvedList = container.Resolve<IList<string>>();
    Assert.AreEqual(list, resolvedList);
}
```

### Builder

One of the most useful design patterns, especially for library builders, was used to reduce `if else` chains and make a pretty internal API.

```csharp
// src/Application/Panels/Options/OptionsBuilder.cs
using System.Collections.ObjectModel;

namespace Journal.Application.Panels.Options;

public class OptionsBuilder
{
    private readonly IDictionary<string, Action> _options = new Dictionary<string, Action>();

    public OptionsBuilder AddOption(string description, Action action)
    {
        _options.Add(description, action);
        return this;
    }

    public OptionsHandler Build() => new(new ReadOnlyDictionary<string, Action>(_options));
}
```

And here's the usage:

```csharp
// src/Application/Panels/EntryAddedState.cs
// ...
public EntryAddedState(IPanelController panelController, JournalEntry entry)
{
    _panelController = panelController;
    _entry = entry;
    _optionsHandler = new OptionsBuilder()
        .AddOption("View entry", HandleViewEntryOption)
        .AddOption("Add tags", HandleAddTagsOption)
        .AddOption("Return to menu", HandleReturnToMenuOption)
        .Build();
}
// ...
```

### Prototype

I used the prototype design pattern on the JournalEntry model. I will use this to implement the action of making a copy of a stored journal entry.

```csharp
// src/Domain/Models/JournalEntry.cs
using Journal.Domain.Models.Abstractions;

namespace Journal.Domain.Models;

public record JournalEntry(
    string Title,
    string Content,
    IList<EntryTag> Tags
) : Entity, ICopyable<JournalEntry>
{
    public DateTime CreatedAt { get; init; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public JournalEntry Copy() => this with
    {
        Title = $"{Title} (Copy)",
        Tags = new List<EntryTag>(Tags),
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now
    };
};
```

```csharp
using Journal.DataAccess.JournalEntries.Abstractions;
using Journal.Domain.Models;
using Journal.Domain.Models.Abstractions;

namespace Journal.DataAccess.JournalEntries;

public class MemoryJournalEntriesRepository : IJournalEntryRepository
{
    private readonly IList<JournalEntry> _journalEntries;

    // ...
    public Task<JournalEntry?> GetById(Guid id)
    {
        return Task.FromResult(_journalEntries.FirstOrDefault(je => je.Id == id));
    }
    // ...
}
```

### Factory (Method)

For the previous laboratory work I had a bit of an unusual layer hierarchy in my application, where DataAccess was below Application. That's because I want to have different implementations of the repositories, with the one used being selected by the user. Luckily, I could refactor the code to use the usual layer hierarchy by using a factory for the repositories.

```csharp
// src/Domain/Factory/RepositoryFactory.cs
using Journal.Domain.Factory.Abstractions;
using Journal.Domain.Models.Abstractions;

namespace Journal.Domain.Factory;

public class RepositoryFactory<BaseRepository, ModelT> : IRepositoryFactory<BaseRepository, ModelT>
    where ModelT : Entity where BaseRepository : IRepository<ModelT>
{
    private readonly IDictionary<string, Func<BaseRepository>> _repositoryThunks;
    public RepositoryFactory()
    {
        _repositoryThunks = new Dictionary<string, Func<BaseRepository>>();
    }
    public void Register(string name, Func<BaseRepository> repositoryThunk)
    {
        _repositoryThunks.Add(name, repositoryThunk);
    }
    public BaseRepository CreateRepository(string name)
    {
        if (!_repositoryThunks.ContainsKey(name))
        {
            throw new KeyNotFoundException($"Repository '{name}' not registered in {nameof(RepositoryFactory<BaseRepository, ModelT>)}");
        }
        return _repositoryThunks[name]();
    }
}
```

with the initialization being the following:

```csharp
// src/DataAccess/DI/Registration.cs
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
```

This way the abstractions can be placed in the Application layer and the implementations in the DataAccess layer.

## Conclusions

For this laboratory work I continued working on my Journal Console Application and added some extra functionality while using creational design patterns. I have used the following patterns: Singleton, Builder, Prototype and Factory. The first three patterns were needed for trivial cases in my application, while the Factory pattern helped me to refactor the separation of concerns in my application. Most dependency injection container libraries offer an included factory, for registering and accessing type instances by key, but since I implemented my own simplified DI container, I used a more traditional and verbose approach.
