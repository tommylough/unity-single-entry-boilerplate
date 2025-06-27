using UnityEngine;
using Services.Audio;
using Services.GameState;

namespace Gameplay
{
    /// <summary>
    /// Example level controller for Level 1
    /// </summary>
    public class Level1Controller : SceneController
    {
        [Header("Level 1 Settings")]
        [SerializeField] private Transform playerSpawnPoint;
        [SerializeField] private AudioClip levelMusic;
        [SerializeField] private AudioClip levelCompleteSound;

        private IAudioService _audioService;
        private IGameStateService _gameStateService;
        private float _levelStartTime;

        protected override void Start()
        {
            base.Start();

            // Get services
            _audioService = GetService<IAudioService>();
            _gameStateService = GetService<IGameStateService>();

            // Initialize level
            InitializeLevel();
        }

        protected override void SubscribeToEvents()
        {
            EventBus.Subscribe<PlayerDiedEvent>(OnPlayerDied);
            EventBus.Subscribe<ItemCollectedEvent>(OnItemCollected);
            EventBus.Subscribe<LevelCompletedEvent>(OnLevelCompleted);
        }

        protected override void UnsubscribeFromEvents()
        {
            EventBus.Unsubscribe<PlayerDiedEvent>(OnPlayerDied);
            EventBus.Unsubscribe<ItemCollectedEvent>(OnItemCollected);
            EventBus.Unsubscribe<LevelCompletedEvent>(OnLevelCompleted);
        }

        private async void InitializeLevel()
        {
            if (enableDebugLogs)
                Debug.Log("[Level1Controller] Initializing Level 1...");

            // Change game state to playing
            _gameStateService.ChangeState(GameState.Playing);

            // Play level music
            if (levelMusic != null)
            {
                await _audioService.PlayMusicAsync(levelMusic);
            }

            // Spawn player if spawn point is set
            if (playerSpawnPoint != null)
            {
                SpawnPlayer();
            }

            // Record start time
            _levelStartTime = Time.time;

            // Publish level started event
            EventBus.Publish(new LevelStartedEvent("Level1"));

            if (enableDebugLogs)
                Debug.Log("[Level1Controller] Level 1 initialized successfully!");
        }

        private void SpawnPlayer()
        {
            // This is where you'd spawn your actual player prefab
            // For now, just create an empty GameObject as an example
            var playerObject = new GameObject("Player");
            playerObject.transform.position = playerSpawnPoint.position;
            playerObject.transform.rotation = playerSpawnPoint.rotation;

            // Add a simple visual representation
            var renderer = playerObject.AddComponent<MeshRenderer>();
            var filter = playerObject.AddComponent<MeshFilter>();
            filter.mesh = CreateCubeMesh();
            
            // You could add player controller components here
            
            EventBus.Publish(new PlayerSpawnedEvent(playerSpawnPoint.position, playerObject));

            if (enableDebugLogs)
                Debug.Log("[Level1Controller] Player spawned at " + playerSpawnPoint.position);
        }

        private void OnPlayerDied(PlayerDiedEvent playerDiedEvent)
        {
            if (enableDebugLogs)
                Debug.Log($"[Level1Controller] Player died at {playerDiedEvent.DeathPosition}");

            // Handle player death - restart level, show game over screen, etc.
            _gameStateService.ChangeState(GameState.GameOver);
        }

        private void OnItemCollected(ItemCollectedEvent itemEvent)
        {
            if (enableDebugLogs)
                Debug.Log($"[Level1Controller] Item collected: {itemEvent.ItemType} x{itemEvent.Quantity}");

            // Handle item collection - update score, play sound, etc.
            // This is where you'd implement your game's collectible logic
        }

        private async void OnLevelCompleted(LevelCompletedEvent levelEvent)
        {
            if (enableDebugLogs)
                Debug.Log($"[Level1Controller] Level completed in {levelEvent.CompletionTime:F2} seconds!");

            // Play completion sound
            if (levelCompleteSound != null)
            {
                await _audioService.PlaySFXAsync(levelCompleteSound);
            }

            // Change game state
            _gameStateService.ChangeState(GameState.Victory);

            // Here you might transition to the next level, show victory screen, etc.
        }

        // Example method that could be called by game logic to complete the level
        public void CompleteLevel()
        {
            var completionTime = Time.time - _levelStartTime;
            EventBus.Publish(new LevelCompletedEvent("Level1", completionTime));
        }

        // Simple utility to create a cube mesh for the example player
        private Mesh CreateCubeMesh()
        {
            var mesh = new Mesh();
            
            // Simple cube vertices
            var vertices = new Vector3[]
            {
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f)
            };

            var triangles = new int[]
            {
                0, 2, 1, 0, 3, 2, // Front
                4, 5, 6, 4, 6, 7, // Back
                0, 1, 5, 0, 5, 4, // Bottom
                3, 7, 6, 3, 6, 2, // Top
                0, 4, 7, 0, 7, 3, // Left
                1, 2, 6, 1, 6, 5  // Right
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            
            return mesh;
        }
    }
}
