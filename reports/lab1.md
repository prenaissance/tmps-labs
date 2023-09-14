# Topic: SOLID

## Author: Andrieș Alexandru

---

## Objectives:

**1. Study and understand the SOLID Principles.**

**2. Choose a domain, define its main classes/models/entities and choose the appropriate instantiation mechanisms.**

**3. Create a sample project that respects SOLID Principles.**

## Achieved goals:

- Chose C# as the programming language and vscode as the IDE. The testing framework MSTest was used, but the no libraries and frameworks rule for business logic was respected and will probably be respected throughout all the labs.

- The following domain was selected: Journal Console Application, and the same domain will be used throughout all the labs.

- SOLID principles were respected and I implemented my own DI container.

## Implementation

Not to use enterprise coding practices in vain, my project grew to a decent size so I would not be ashamed of over-engineering. The project is structured in the following hierarchy:

- Client - registration of dependencies and starting the program
  - Application - the main logic and glue of the system
    - DataAccess - repository implementations
      - Domain - Models and some common abstractions

It's a bit different to the standard project structures, because the application layer has access to data access, instead of the other way around. This decision was made, because the app gives the option to choose between storage options as a strategy.

The functionality is far from complete, at the moment the UI menu is complete, the state machine, dependency injection, models and the in memory repository, but some parts are not glued together. You can check the output by following the instructions in the [README](../README.md).

## SOLID principles examples

### Single Responsibility Principle

The "Panels" in the app are implemented as separate states, the options in the menu of each panel are implemented as separate higher or functions (Strategy pattern in this case). This makes the code more readable and maintainable. There is also a separation of concerns between the UI and the business logic - The app is structured to have a state controller which changes the states, the states which handle the business logic and the Views which handle the UI (in this case the console).

```csharp
// src/Application/Panels/Options/OptionsHandler.cs
namespace Journal.Application.Panels.Options;

public class OptionsHandler
{
    private readonly IDictionary<string, Action> _options;

    public OptionsHandler(IDictionary<string, Action> options)
    {
        _options = options;
    }
    public IList<string> Options => _options.Keys.ToList();

    public void HandleOption(string option)
    {
        if (_options.ContainsKey(option))
        {
            _options[option]();
        }
        else
        {
            throw new KeyNotFoundException($"Option '{option}' not registered");
        }
    }
}
```

### Open-Closed Principle

In the following example, I used a builder pattern to be able to add new options in the future without having to edit the `if else` chain.

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
// src/Application/Panels/WelcomeState.cs
// ...
public WelcomeState(IPanelController panelController)
    {
        _panelController = panelController;
        _optionsHandler = new OptionsBuilder()
            .AddOption("In memory", HandleMemoryStorageOption)
            .AddOption("In file storage (WIP)", HandleFileStorageOption)
            .Build();
    }
// ...
```

### Liskov Substitution Principle

For the used models I inherited the `Entity` class. This is important, as ensuring that every entity has the base entities' UUID Id allows me to safely store the polymorphic entities in the same repository and make the repository be able to search by Id.

```csharp
// src/Domain/Abstraction/Entity.cs
namespace Journal.Domain.Models.Abstractions
{
    public abstract record Entity
    {
        public Guid Id { get; init; }
    }
}
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

### Interface Segregation Principle

The `JournalEntry` model implements both the `Entity` abstract class and the `ICopyable<T>` interface. Not all entities need to be copyable, but in this case it makes sense to be able to copy a journal entry.

```csharp
// src/Domain/Models/JournalEntry.cs
using Journal.Domain.Models.Abstractions;

namespace Journal.Domain.Models;

public record JournalEntry(
    string Title,
    string Content,
    IList<EntryTag> Tags,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    Guid UserId
) : Entity, ICopyable<JournalEntry>
{
    public JournalEntry Copy() => this with
    {
        Title = $"{Title} (Copy)",
        Tags = new List<EntryTag>(Tags)
    };
};
```

### Dependency Inversion Principle

The dependency inversion principle is respected by the use of dependency injection. Many classes accept interfaces or class instances in their constructors, which helps with loose coupling and polymorphism. Here's a snippet of the registration of dependencies from the Application layer:

```csharp
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
```

The DI container implementation is in [this file](../src/Application/DI/Container/DIContainer.cs).

## Conclusions

For this laboratory work I bootstrapped a project and used SOLID principles, by starting development on a Journal console application. I had to grow the application to a decent size and use some architectural patterns to be able to showcase the principles in a meaningful way. I also implemented my own DI container, which made me study type reflections a bit.
