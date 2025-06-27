using System.Threading.Tasks;

namespace Core.SceneManagement
{
    /// <summary>
    /// Interface for async scene management operations
    /// </summary>
    public interface ISceneManager
    {
        /// <summary>
        /// Load a scene asynchronously, replacing the current scene
        /// </summary>
        Task LoadSceneAsync(string sceneName);

        /// <summary>
        /// Load a scene additively (without unloading current scenes)
        /// </summary>
        Task LoadSceneAdditiveAsync(string sceneName);

        /// <summary>
        /// Unload a scene asynchronously
        /// </summary>
        Task UnloadSceneAsync(string sceneName);

        /// <summary>
        /// Get the currently active scene name
        /// </summary>
        string GetActiveSceneName();

        /// <summary>
        /// Check if a scene is currently loaded
        /// </summary>
        bool IsSceneLoaded(string sceneName);

        /// <summary>
        /// Event triggered when scene loading starts
        /// </summary>
        event System.Action<string> OnSceneLoadStarted;

        /// <summary>
        /// Event triggered when scene loading completes
        /// </summary>
        event System.Action<string> OnSceneLoadCompleted;

        /// <summary>
        /// Event triggered when scene unloading starts
        /// </summary>
        event System.Action<string> OnSceneUnloadStarted;

        /// <summary>
        /// Event triggered when scene unloading completes
        /// </summary>
        event System.Action<string> OnSceneUnloadCompleted;
    }
}
