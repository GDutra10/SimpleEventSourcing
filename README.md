# SimpleEventSourcing

A lightweight and extensible **Event Sourcing** library for .NET applications, designed to handle event-driven architectures. This library provides an easy-to-use implementation of **event storage, projections, and event handlers**, making it ideal for domain-driven design (DDD) and CQRS patterns.

## 🚀 Features
- **Event Sourcing** – Stores domain events instead of current state.
- **Projections** – Automatically updates the latest state from events.
- **Extensible Event Handlers** – Supports dependency injection.
- **Async Repositories** – Works with different data stores.

## 🔧 Usage

### 1️⃣ Define an Event

```csharp
public class UserCreatedEvent : Event
{
    public override Guid StreamId => Id;
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
```

### 2️⃣ Create a Projection

```csharp
public class User : IProjection
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
```

### 3️⃣ Implement an Event Handler

```csharp
public class UserCreateHandler : IEventHandler<UserCreatedEvent, User>
{
    public void Handle(User projection, UserCreatedEvent e)
    {
        projection.Id = e.StreamId;
        projection.Name = e.Name;
        projection.Email = e.Email;
    }
}
```

### 4️⃣ Set Up Event Source

```csharp
var eventHandlers = new List<IEventHandler<UserCreatedEvent, User>> { new UserCreateHandler() };
var eventSourceConfiguration = new EventSourcingConfiguration();
var eventSource = new EventSource<User, UserCreatedEvent>(eventHandlers, projectionRepository, eventRepository, eventConfiguration);
```

or 

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.UseEventSourcing(config => { config.UseInMemory = true; });
builder.Services.AddTransient<IEventHandler<UserCreatedEvent, User>, UserCreateHandler>();
```

### 5️⃣ Append Events

```csharp
var newEvent = new UserCreatedEvent
{
    UserId = Guid.NewGuid(),
    Name = "John Doe",
    Email = "john.doe@example.com"
};

await eventSource.AppendAsync(newEvent, default);
```

### 6️⃣ Retrieve State

```csharp
var user = await eventSource.GetAsync(newEvent.StreamId, CancellationToken.None);
Console.WriteLine($"User: {user?.Name}, Email: {user?.Email}");
```

## 📂 Examples

Check out the Example folder in the repository for detailed usage examples and implementations.
