# CoffeeShopApp

A deliberately small .NET 8 console application for learning SOLID and
multithreading without adding abstractions that do not provide value.

## What is included?

- 3 coffee machines
- 6 drinks: Espresso, Americano, Cappuccino, Latte, Mocha, Tea
- No cooks
- No recipes
- No inventory
- No restocking
- No order repository
- One notification repository
- JSON notification persistence
- `Thread` for concurrent drink preparation
- `lock` for shared machine state
- `Queue<T>` for notifications
- `AutoResetEvent` to wake the notification worker

## Structure

```text
CoffeeShopApp
├── Program.cs
├── Models
│   ├── Coffee.cs
│   ├── CoffeeMachine.cs
│   └── Notification.cs
├── Enums
│   ├── CoffeeType.cs
│   └── NotificationType.cs
├── Repositories
│   ├── INotificationRepository.cs
│   └── JsonNotificationRepository.cs
├── Services
│   ├── MenuService.cs
│   ├── CoffeeMachineService.cs
│   ├── NotificationService.cs
│   └── NotificationWorker.cs
└── Views
    └── ConsoleView.cs
```

## Why only one repository?

The only data that this simplified application needs to persist is
**notifications**.

Orders are temporary runtime objects represented by an order ID. There is no
requirement to maintain order history, so creating an `Order` model and an
`OrderRepository` would add unnecessary complexity.

The menu is also static application data, so `MenuService` is enough.

This is an important design lesson:

> Do not create a repository merely because an entity exists.

A repository is useful when the application has a meaningful persistence
boundary.

## Threading

Each accepted order gets its own processing thread.

```text
ConsoleView
    |
    v
CoffeeMachineService
    |
    +---- Machine 1 ---- Order Thread
    +---- Machine 2 ---- Order Thread
    +---- Machine 3 ---- Order Thread
                              |
                              v
                    NotificationService
                         Queue + lock
                              |
                        AutoResetEvent
                              |
                              v
                    NotificationWorker
                              |
                              v
                           Console
```

`lock` protects machine selection and reservation so two concurrent orders
cannot reserve the same machine.

The notification queue is protected by another lock because multiple order
threads can publish notifications concurrently.

`AutoResetEvent` wakes the notification worker when a notification arrives.

`ManualResetEventSlim` is intentionally not used because the simplified
application no longer has a persistent condition such as restocking.

## SOLID

### SRP

- `MenuService` -> menu data
- `CoffeeMachineService` -> machine allocation and order processing
- `NotificationService` -> notification queue and publishing
- `NotificationWorker` -> notification consumption
- `JsonNotificationRepository` -> notification persistence
- `ConsoleView` -> user interaction

### DIP

`NotificationService` depends on `INotificationRepository`, not directly on
the JSON implementation.

`Program.Main()` wires the concrete implementation.

### ISP

The repository interface contains only the operations actually needed for
notification persistence.

### OCP / LSP

A different notification persistence implementation can replace
`JsonNotificationRepository` without changing `NotificationService`.

## Running

Open `CoffeeShopApp.sln` in Visual Studio 2022 and run the project.

The application creates:

```text
Data/notifications.json
```

under the application's output directory.
