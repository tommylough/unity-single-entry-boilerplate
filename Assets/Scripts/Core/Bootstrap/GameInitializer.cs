using System.Threading.Tasks;
using UnityEngine;
using Core.Services;
using Core.Events;
using Core.SceneManagement;
using Services.Audio;
using Services.SaveLoad;
using Services.GameState;
using Services.UI;

namespace Core.Bootstrap
{
    /// <summary>
    /// Main game bootstrap component that initializes all core systems and services.
    /// This is the single entry point for the entire application.
    /// </summary>
    public class GameInitializer : MonoBehaviour
    {
        [Header("Core Prefabs")]
        [SerializeField] private Camera mainCameraPrefab;
        [SerializeField] private Light mainLightPrefab;
        [SerializeField] private Canvas uiCanvasPrefab;
        
        [Header("Game Settings")]
        [SerializeField] private string firstSceneName = "Level1";
        [SerializeField] private AudioClip backgroundMusic;
        
        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = true;
        
        private IServiceContainer _serviceContainer;
        private IEventBus _eventBus;
        private ISceneManager _sceneManager;

        private async void Start()
        {
            await InitializeGameAsync();
        }

        /// <summary>
        /// Main initialization sequence for the entire game
        /// </summary>
        private async Task InitializeGameAsync()
        {
            if (enableDebugLogs) Debug.Log("[GameInitializer] Starting game initialization...");

            try
            {
                // 1. Initialize core systems
                await InitializeCoreSystemsAsync();
                
                // 2. Register all services
                await RegisterServicesAsync();
                
                // 3. Initialize services
                await InitializeServicesAsync();
                
                // 4. Setup core game objects
                await SetupCoreGameObjectsAsync();
                
                // 5. Start background music if provided
                if (backgroundMusic != null)
                {
                    var audioService = _serviceContainer.Get<IAudioService>();
                    await audioService.PlayMusicAsync(backgroundMusic);
                }
                
                // 6. Load first scene
                await LoadFirstSceneAsync();
                
                if (enableDebugLogs) Debug.Log("[GameInitializer] Game initialization completed successfully!");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[GameInitializer] Failed to initialize game: {ex.Message}");
                Debug.LogException(ex);
            }
        }

        /// <summary>
        /// Initialize core systems (Service Container, Event Bus, Scene Manager)
        /// </summary>
        private async Task InitializeCoreSystemsAsync()
        {
            if (enableDebugLogs) Debug.Log("[GameInitializer] Initializing core systems...");
            
            // Initialize service container
            _serviceContainer = new ServiceContainer();
            ServiceLocator.Initialize(_serviceContainer);
            
            // Initialize event bus
            _eventBus = new EventBus();
            _serviceContainer.Register<IEventBus>(_eventBus);
            
            // Initialize scene manager
            _sceneManager = new AsyncSceneManager();
            _serviceContainer.Register<ISceneManager>(_sceneManager);
            
            await Task.CompletedTask; // For future async initialization
        }

        /// <summary>
        /// Register all services in the dependency injection container
        /// </summary>
        private async Task RegisterServicesAsync()
        {
            if (enableDebugLogs) Debug.Log("[GameInitializer] Registering services...");
            
            // Register core services
            _serviceContainer.Register<IAudioService, AudioService>();
            _serviceContainer.Register<ISaveLoadService, JsonSaveLoadService>();
            _serviceContainer.Register<IGameStateService, GameStateService>();
            _serviceContainer.Register<IUIService, UIService>();
            
            await Task.CompletedTask;
        }

        /// <summary>
        /// Initialize all registered services
        /// </summary>
        private async Task InitializeServicesAsync()
        {
            if (enableDebugLogs) Debug.Log("[GameInitializer] Initializing services...");
            
            // Get and initialize each service
            var audioService = _serviceContainer.Get<IAudioService>();
            var saveLoadService = _serviceContainer.Get<ISaveLoadService>();
            var gameStateService = _serviceContainer.Get<IGameStateService>();
            var uiService = _serviceContainer.Get<IUIService>();
            
            // Initialize services that need async setup
            if (audioService is AudioService audioServiceImpl)
            {
                await audioServiceImpl.InitializeAsync();
            }
            
            if (saveLoadService is JsonSaveLoadService saveLoadServiceImpl)
            {
                await saveLoadServiceImpl.InitializeAsync();
            }
            
            if (gameStateService is GameStateService gameStateServiceImpl)
            {
                await gameStateServiceImpl.InitializeAsync();
            }
            
            if (uiService is UIService uiServiceImpl)
            {
                await uiServiceImpl.InitializeAsync();
            }
        }

        /// <summary>
        /// Setup essential game objects (Camera, Light, UI Canvas)
        /// </summary>
        private async Task SetupCoreGameObjectsAsync()
        {
            if (enableDebugLogs) Debug.Log("[GameInitializer] Setting up core game objects...");
            
            // Instantiate main camera if provided
            if (mainCameraPrefab != null)
            {
                var cameraInstance = Instantiate(mainCameraPrefab);
                cameraInstance.name = "Main Camera";
                DontDestroyOnLoad(cameraInstance.gameObject);
            }
            
            // Instantiate main light if provided
            if (mainLightPrefab != null)
            {
                var lightInstance = Instantiate(mainLightPrefab);
                lightInstance.name = "Main Light";
                DontDestroyOnLoad(lightInstance.gameObject);
            }
            
            // Instantiate UI canvas if provided
            if (uiCanvasPrefab != null)
            {
                var canvasInstance = Instantiate(uiCanvasPrefab);
                canvasInstance.name = "UI Canvas";
                DontDestroyOnLoad(canvasInstance.gameObject);
                
                // Register canvas with UI service
                var uiService = _serviceContainer.Get<IUIService>();
                if (uiService is UIService uiServiceImpl)
                {
                    uiServiceImpl.SetMainCanvas(canvasInstance);
                }
            }
            
            // Keep this GameObject alive across scenes
            DontDestroyOnLoad(gameObject);
            
            await Task.CompletedTask;
        }

        /// <summary>
        /// Load the first game scene
        /// </summary>
        private async Task LoadFirstSceneAsync()
        {
            if (enableDebugLogs) Debug.Log($"[GameInitializer] Loading first scene: {firstSceneName}");
            
            if (!string.IsNullOrEmpty(firstSceneName))
            {
                await _sceneManager.LoadSceneAsync(firstSceneName);
            }
            else
            {
                Debug.LogWarning("[GameInitializer] No first scene specified!");
            }
        }

        private void OnDestroy()
        {
            // Clean up when this object is destroyed
            ServiceLocator.Reset();
            
            if (enableDebugLogs) Debug.Log("[GameInitializer] Game initializer destroyed and services cleaned up.");
        }
    }
}
