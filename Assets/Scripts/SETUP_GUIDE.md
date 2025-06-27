# Unity 6 Single Entry Boilerplate - Setup Guide

## Quick Start

1. **Set Bootstrap as startup scene**
   - Add `Bootstrap` scene to Build Settings as index 0
   - Add `Level1` scene as index 1
   - Set Bootstrap as the active scene

2. **Configure GameInitializer**
   - In Bootstrap scene, select the GameInitializer GameObject
   - Set `_firstLevelScene` to "Level1"
   - Assign prefabs for Camera, Light, and UI Canvas (optional)

3. **Create prefabs (optional)**
   - Create prefabs for Main Camera, Main Light, and UI Canvas
   - Assign them to GameInitializer's SerializeFields
   - These will be instantiated and made persistent across scenes

4. **Run the project**
   - Press Play in Bootstrap scene
   - The system will initialize services and load Level1

## Adding New Services

1. Create interface in `Assets/Scripts/Services/YourService/`
2. Implement the interface
3. Register in `GameInitializer.InitializeServicesAsync()`

Example:
```csharp
// 1. Create interface
public interface IInventoryService
{
    Task AddItemAsync(string itemId);
}

// 2. Implement
public class InventoryService : IInventoryService
{
    public async Task AddItemAsync(string itemId)
    {
        // Implementation
    }
}

// 3. Register in GameInitializer
var inventoryService = new InventoryService();
_serviceContainer.Register<IInventoryService>(inventoryService);
```

## Creating New Levels

1. Create new scene
2. Add a GameObject with a script inheriting from `SceneController`
3. Override `SubscribeToEvents()` and `UnsubscribeFromEvents()`
4. Add scene to Build Settings
5. Load via `await SceneManager.LoadSceneAsync("SceneName")`

## Using Events

1. Create events in `GameplayEvents.cs` or create new event files
2. Publish: `EventBus.Publish(new YourEvent())`
3. Subscribe: `EventBus.Subscribe<YourEvent>(OnYourEvent)`
4. Unsubscribe: `EventBus.Unsubscribe<YourEvent>(OnYourEvent)`

## Architecture Principles

- **Single Entry**: Everything starts from Bootstrap scene
- **Async/Await**: Use modern async patterns
- **No Singletons**: Use dependency injection
- **Event-Driven**: Communicate through events
- **Assembly Definitions**: Organized code compilation
- **Service Container**: Centralized dependency management

## File Structure Summary

```
Core/
├── Bootstrap/GameInitializer.cs     - Main initialization
├── Events/                          - Event system
├── Services/                        - DI container & locator
├── SceneManagement/                 - Async scene loading
└── Utilities/                       - Helper extensions

Services/
├── Audio/                           - Audio management
├── SaveLoad/                        - JSON persistence
├── GameState/                       - Game state & pausing
└── UI/                             - UI management

Gameplay/
├── SceneController.cs              - Base scene controller
├── Level1Controller.cs             - Example level
├── PlayerController.cs             - Example player
└── GameplayEvents.cs               - Game-specific events
```

## Best Practices

1. Always inherit scene controllers from `SceneController`
2. Use async/await for long-running operations
3. Communicate between systems via events
4. Register all services in GameInitializer
5. Use SerializeFields for bindings, not FindObjectOfType
6. Keep services stateless when possible
7. Unsubscribe from events in OnDestroy

## Testing

Use Context Menu items in Level1Controller:
- "Test Save Data" - Tests save system
- "Test Load Data" - Tests load system
- "Pause Game" - Tests game state management
- "Resume Game" - Tests game state management

## Extending

The system is designed to be modular. Add new:
- Services for game-specific functionality
- Events for custom communication
- Scene controllers for different levels
- Utilities for common operations

Each assembly definition allows for clean separation of concerns and faster compilation times.
