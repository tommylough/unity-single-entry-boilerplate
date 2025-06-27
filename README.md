# Unity 6 Single Entry Boilerplate

A modern Unity 6 boilerplate project featuring a single entry point architecture with async/await patterns, dependency injection, and event-driven design.

## Architecture Overview

This boilerplate implements a clean, scalable architecture with the following principles:

- **Single Entry Point**: All game initialization happens through the Bootstrap scene
- **Async/Await**: Modern async patterns instead of coroutines
- **No Singletons**: Dependency injection through service container
- **Event-Driven**: C# events for decoupled communication
- **Assembly Definitions**: Proper code organization and faster compilation
- **Bindings**: SerializeField-based dependency binding
- **Services**: Modular service architecture

## Project Structure

```
Assets/
├── Scripts/
│   ├── Core/                    (Core.asmdef)
│   │   ├── Bootstrap/
│   │   │   └── GameInitializer.cs
│   │   ├── Events/
│   │   │   ├── IEventBus.cs
│   │   │   ├── EventBus.cs
│   │   │   └── IGameEvent.cs
│   │   ├── Services/
│   │   │   ├── IServiceContainer.cs
│   │   │   ├── ServiceContainer.cs
│   │   │   └── ServiceLocator.cs
│   │   └── SceneManagement/
│   │       ├── ISceneManager.cs
│   │       └── AsyncSceneManager.cs
│   ├── Services/                (Services.asmdef)
│   │   ├── Audio/
│   │   │   ├── IAudioService.cs
│   │   │   └── AudioService.cs
│   │   ├── SaveLoad/
│   │   │   ├── ISaveLoadService.cs
│   │   │   └── JsonSaveLoadService.cs
│   │   └── GameState/
│   │       ├── IGameStateService.cs
│   │       └── GameStateService.cs
│   └── Gameplay/                (Gameplay.asmdef)
│       ├── SceneController.cs
│       └── Level1Controller.cs
├── Scenes/
│   ├── Bootstrap.unity          (Main entry point)
│   └── Level1.unity             (Example level)
└── Settings/                    (URP settings)
```

## How to Use

### 1. Setup Bootstrap Scene

1. Set `Bootstrap` as your first scene in Build Settings
2. The `GameInitializer` prefab in Bootstrap scene will:
   - Initialize all services
   - Set up core game objects (Camera, Lights, UI Canvas)
   - Load the first level

### 2. Creating Services

All services implement interfaces and are registered in the service container:

```csharp
// 1. Create interface
public interface IMyService
{
    Task DoSomethingAsync();
}

// 2. Implement service
public class MyService : IMyService
{
    public async Task DoSomethingAsync()
    {
        // Implementation
    }
}

// 3. Register in GameInitializer
_serviceContainer.Register<IMyService, MyService>();
```

### 3. Creating Scene Controllers

Each scene should have a controller that inherits from `SceneController`:

```csharp
public class MyLevelController : SceneController
{
    protected override void SubscribeToEvents()
    {
        EventBus.Subscribe<GameStateChangedEvent>(OnGameStateChanged);
    }

    protected override void UnsubscribeFromEvents()
    {
        EventBus.Unsubscribe<GameStateChangedEvent>(OnGameStateChanged);
    }

    private void OnGameStateChanged(GameStateChangedEvent gameEvent)
    {
        // Handle event
    }
}
```

### 4. Using Events

Create events by implementing `IGameEvent`:

```csharp
public class PlayerDiedEvent : IGameEvent
{
    public Vector3 DeathPosition { get; }
    public PlayerDiedEvent(Vector3 position) => DeathPosition = position;
}

// Publish event
EventBus.Publish(new PlayerDiedEvent(transform.position));

// Subscribe to event
EventBus.Subscribe<PlayerDiedEvent>(OnPlayerDied);
```

### 5. Async Scene Loading

Use the async scene manager for smooth scene transitions:

```csharp
await SceneManager.LoadSceneAsync("NextLevel");
await SceneManager.LoadSceneAdditiveAsync("UI");
await SceneManager.UnloadSceneAsync("OldLevel");
```

## Available Services

### Audio Service
- Play SFX and music with volume control
- Async audio playback
- Master/Music/SFX volume separation

### Save/Load Service
- JSON-based save system
- Async file operations
- Persistent data storage

### Game State Service
- Centralized game state management
- Pause/Resume functionality
- State change events

### Scene Manager
- Async scene loading
- Additive scene support
- Scene transition management

## Key Features

### 🔄 Async/Await Pattern
All major operations use async/await instead of coroutines for better performance and readability.

### 🏗️ Dependency Injection
Services are injected through a container, making testing and swapping implementations easy.

### 📡 Event System
Loosely coupled communication through a type-safe event bus.

### 🧩 Assembly Definitions
Organized code with proper dependencies and faster compilation times.

### 🎯 Bindings
Use SerializeFields in GameInitializer to bind prefabs, then instantiate them programmatically.

### 🚫 No Singletons
Clean architecture without static dependencies or singleton anti-patterns.

## Build Settings

1. Add scenes in this order:
   - `Bootstrap` (index 0)
   - `Level1` (index 1)
   - Additional levels...

2. Set `Bootstrap` as the startup scene

## Getting Started

1. Open the Bootstrap scene
2. Configure the GameInitializer:
   - Set the first level scene name
   - Assign prefabs for Camera, Light, UI Canvas
   - Add background music if desired
3. Create your level scenes with SceneController components
4. Build and run!

## Extending the System

- Add new services by implementing interfaces and registering them
- Create custom events by implementing `IGameEvent`
- Build level-specific controllers by inheriting from `SceneController`
- Add new async operations using the established patterns

This architecture provides a solid foundation for scalable Unity projects with modern C# patterns and clean separation of concerns.
