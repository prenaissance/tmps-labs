# Topic: Structural Design Patterns

## Author: Andrieș Alexandru

---

## Objectives:

**1. Study and understand the Structural Design Patterns.**

**2. As a continuation of the previous laboratory work, think about the functionalities that your system will need to provide to the user.**

**3. Implement some additional functionalities, or create a new project using structural design patterns.**

## Achieved goals:

- Continued developing the previously chosen topic of a Journal Console Application. Made the following upgrades:

  - Refactored where needed
  - Added UI support for checkboxes, feedback view messages and flushing the console
  - Greatly improved the Console UI
  - Added support for creating and adding tags to journal entries

- Implemented structural design patterns.

## Implementation

I extended the previous laboratory work with improvements and new features, while adding structural design patterns where they were missing. At the moment, the application is quite usable, but doesn't have the option to persist the data implemented and editing the data is not possible. These will be the harder parts of the application, needing marshalling/unmarshalling and to implement a lightweight text editor. I will try to add these features for the next laboratory work, although most of the Behavioral Design Patterns are already implemented.

Extra info and commands for the project can be found in the [README](../README.md).

## Structural Design Patterns Used

### Facade

The design pattern with a 1 to 1 relationship to the OOP principle of encapsulation was obviously extensively used in my application. An example is the class `OptionMenuView`. It is initiated with a list of options and a callback function for when an option is selected and it exposes a single method - `Render`. Internally it does a lot of things - it keeps track of the selected index, it listens to key presses, it calculates how many lines the menu takes up and flushes them before each re-render and it iterates through the options to see which one should be colored green for selecting it. But it exposes a simple reusable API.

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

### Bridge

Another simple design patterns, with a 1 to one relationship with OOP's abstraction principle. Impossible to omit when using dependency injection. In this project for example, I have an abstraction of a repository in the Application Layer:

```csharp
// src/Application/JournalEntries/Abstractions/IJournalEntryRepository.cs
public interface IJournalEntryRepository : IRepository<JournalEntry>
{
    Task<IList<JournalEntry>> GetManyByTagName(string tag);
    Task<IList<JournalEntry>> GetManyByTextSearch(string text);
}
```

And one of the implementations (at the moment the only one) is in the DataAccess Layer, which is also a different assembly:

```csharp
// src/DataAccess/JournalEntries/MemoryJournalEntryRepository.cs
public class MemoryJournalEntryRepository : IJournalEntryRepository
{
    // ...
}
```

With the registration happening in the DI Container:

```csharp
// src/DataAccess/DI/Registration.cs
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

This allows me to never edit the classes that utilize the repository when I edit the implementation, because they rely on the abstraction and are also not the ones to instantiate the repository, thanks to dependency inversion.

### Flyweight

A more niche optimization pattern, which I used for the tags in my application. Since tags are objects that are shared between many entries, I decided to use the flyweight pattern here. This makes it so less memory is used and the equality of the tags can be checked by reference, but realistically the performance gain is negligible.

```csharp
// src/Application/Tags/TagFactory.cs
public class TagFactory : ITagFactory
{
    // should ideally be a weak dictionary
    private readonly Dictionary<string, EntryTag> _instancesDictionary = new() {
        { $"Important:{ConsoleColor.Red}", new EntryTag("Important", ConsoleColor.Red) }
    };
    public IList<EntryTag> Instances => _instancesDictionary.Values.ToArray();

    public EntryTag GetTag(string name, ConsoleColor consoleColor)
    {
        string hashKey = $"{name}:{consoleColor}";
        if (_instancesDictionary.ContainsKey(hashKey))
        {
            return _instancesDictionary[hashKey];
        }
        EntryTag tag = new(name, consoleColor);
        _instancesDictionary.Add(hashKey, tag);
        return tag;
    }
    public void AddTag(string name, ConsoleColor consoleColor)
    {
        GetTag(name, consoleColor);
    }
}
```

### Decorator

The decorator pattern is used in my application for views. With decorators I can intercept the `Render` method and add extra functionality to it, like clearing the console before rendering or adding a feedback message. This is a very powerful pattern, but is usually used with streams or external libraries. In my case, there were easier ways, but it does add some value.

Note for the examples: `IPanelState` implements `IView`;

```csharp
// src/Application/Views/Decorator/WelcomeMessageViewDecorator.cs
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
```

There's also a self-explanatory `ClearConsoleViewDecorator`. The usage is the following:

```csharp
// src/Application/Panels/States/CreateTagState.cs
IPanelState nextState = _stateFactory.CreateState<NextStateT>();
string welcomeMessage = $"Tag '{_name}' added".ToColor(ConsoleColor.Green);
var newState = new ClearConsoleViewDecorator(
    new WelcomeMessageViewDecorator(nextState, welcomeMessage)
);
_panelController.ChangeState(newState);
```

And the result, allowing a state to be reused in different places with altered results. The following image shows the decorated `MenuState` changed after creating a tag from the menu:

![feedback message decorator](./images/feedback_message_decorator.png)

### Repository

The repository pattern is used in my application for the journal entries. It is a simple abstraction over the data access layer, which allows me to easily swap the source of the data.

```csharp
// src/Domain/Abstractions/Models/IRepository.cs
public interface IRepository<T> where T : Entity
{
    Task<T?> GetById(Guid id);
    Task<IList<T>> GetAll();
    Task<T> Add(T entity);
    Task<T> Update(T entity);
    Task<T> Delete(Guid id);
}
```

```csharp
// src/Application/JournalEntries/Abstractions/IJournalEntryRepository.cs
public interface IJournalEntryRepository : IRepository<JournalEntry>
{
    Task<IList<JournalEntry>> GetManyByTagName(string tag);
    Task<IList<JournalEntry>> GetManyByTextSearch(string text);
}
```

## Conclusions

For this laboratory work I continued developing the previously chosen topic of a Journal Console Application. I improved my application's feature set and implemented structural design patterns. I added the Flyweight and Decorator patterns, and the rest were already implemented, because they are more trivial. The patterns were used for the following features in my application: Tags, Console Views, Data Access. The application is quite usable at the moment, but doesn't have persistance implemented. This will be the focus of the next laboratory work, along with other features.
