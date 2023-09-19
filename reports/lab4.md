# Topic: Behavioral Design Patterns

## Author: Andrieș Alexandru

---

## Objectives:

**1. Study and understand the Behavioral Design Patterns.**

**2. As a continuation of the previous laboratory work, think about what communication between software entities might be involved in your system.**

**3. Create a new Project or add some additional functionalities using behavioral design patterns.**

## Achieved goals:

- Continued developing the previously chosen topic of a Journal Console Application. Made the following upgrades:

  - Improved support for tags. Now they are colored properly in different locations.
  - Added implementation of repository in file storage
  - Implemented the following additional actions for journal entries:
    - Deleting
    - Copying
  - Added option to seed the application with some data

- Implemented behavioral design patterns.

## Implementation

I extended the previous laboratory work with improvements and new features, while adding and documenting behavioral design patterns. The application can be considered complete, but I omitted some of the more ambitious features I had in plan: a lightweight text editor, editing journal entry content and searching by text interactively. The requirements for the labs though are completed, so I will stop here.

Extra info and commands for the project can be found in the [README](../README.md).

## Behavioral Design Patterns Used

### Command

The following design pattern is often used for applications with a UI, to make extensible components. I tried to replicate the use case for my console application and successfully separated presentation and business logic. The following pattern shows how my option menu receives a lambda function which handles the option selected. This allows me to reuse the menu in different places, with different actions for each option and keep things simple.

```csharp
// src/Application/Views/OptionMenuView.cs
public class OptionMenuView : IView
{
    private int _currentIndex = 0;
    private readonly IList<string> _options;
    private readonly Action<string> _onOptionSelected;
    private void HandleKeyUp()
    {
        // ...
    }
    private void HandleKeyDown()
    {
        // ...
    }
    private void HandleEnter()
    {
        _onOptionSelected(_options[_currentIndex]);
    }
    private readonly ReadOnlyDictionary<ConsoleKey, Action> _keyHandlers;

    public OptionMenuView(IList<string> options, Action<string> onOptionSelected)
    {
        _options = options;
        _onOptionSelected = onOptionSelected;
        _keyHandlers = new ReadOnlyDictionary<ConsoleKey, Action>(
            new Dictionary<ConsoleKey, Action>
            {
                { ConsoleKey.UpArrow, HandleKeyUp },
                { ConsoleKey.DownArrow, HandleKeyDown },
                { ConsoleKey.Enter, HandleEnter }
            }
        );
    }

    private void ListenForInput()
    {
        // ...
    }

    public void Render()
    {
        // ...
        ListenForInput();
    }
}
```

### State

The state pattern is used in my application to have a state machine and restrict the control flow between different panels. It helps keeping the logic simple and the code low-coupled and highly cohesive. The following is the state machine class for the application, that every state has a reference to:

```csharp
// src/Application/Panels/PanelController.cs
public class PanelController : IPanelController
{
    public IPanelState? CurrentState { private get; set; }
    public void ChangeState(IPanelState newState)
    {
        CurrentState = newState;
        Loop();
    }

    public void Loop()
    {
        if (CurrentState == null)
        {
            throw new InvalidOperationException("Initial state not initiated");
        }
        CurrentState.Render();
    }
}
```

And the following is an example of a state transition:

```csharp
// src/Application/Panels/States/MenuState.cs
private void HandleAddEntryOption()
{
    var newState = new ClearConsoleViewDecorator(_stateFactory.CreateState<AddEntryState>());
    _panelController.ChangeState(newState);
}
```

### Strategy

My application supports different storage options for the journal entries. The strategy pattern is used to allow the user to choose the storage option at runtime. The following is the interface for the strategy:

```csharp
// src/Application/JournalEntries/Abstractions/IJournalEntryRepository.cs
public interface IJournalEntryRepository : IRepository<JournalEntry>
{
    Task<IList<JournalEntry>> GetManyByTagName(string tag);
    Task<IList<JournalEntry>> GetManyByTextSearch(string text);
}
```

And it has 2 implementations:

- `MemoryJournalEntryRepository` - stores the entries in memory, for the duration of the application
- `FileJournalEntryRepository` - stores the entries in a json file, for persistence

They are used in the following way:

```csharp
// src/DataAccess/DI/Registration.cs
container
    .RegisterTransient<MemoryJournalEntryRepository>()
    .RegisterTransient<FileJournalEntryRepository>();

RepositoryFactory<IJournalEntryRepository, JournalEntry> repositoryFactory = new();
repositoryFactory.Register(
    WelcomeState.Options.MemoryStorage,
    container.Resolve<MemoryJournalEntryRepository>);
repositoryFactory.Register(
    WelcomeState.Options.FileStorage,
    container.Resolve<FileJournalEntryRepository>);
```

And the user is prompted with the choice when he starts the application:

![storage strategy](./images/storage_strategy.png)

### Visitor

There are a few ways to implement the visitor pattern, which depend on the capabilities of the chosen programming language. The simplest way, with method overloading, is used in my app for the dependency injection container. The following is an example of the same method used in 3 different ways:

```csharp
PanelController panelController = new PanelController();
container
    .RegisterSingleton<IPanelController>(panelController)
    .RegisterSingleton<IStateFactory>(new StateFactory(container))
    .RegisterSingleton<ITagFactory, TagFactory>()
    .RegisterSingleton<JournalEntriesSeeding>();
```

## Conclusions

For this laboratory work I finished developing the previously chosen topic of a Journal Console Application. I improved my application's feature set and implemented behavioral design patterns. I documented the used patterns in my application: Command, State, Strategy and Visitor. I also studied the other behavioral design patterns and I realized that my application could have taken use of the Mediator pattern to reduce communication logic between the states and the registered services.
