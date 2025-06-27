using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Core.Utilities;

namespace Core.SceneManagement
{
    /// <summary>
    /// Async scene manager implementation using modern async/await patterns
    /// </summary>
    public class AsyncSceneManager : ISceneManager
    {
        public event System.Action<string> OnSceneLoadStarted;
        public event System.Action<string> OnSceneLoadCompleted;
        public event System.Action<string> OnSceneUnloadStarted;
        public event System.Action<string> OnSceneUnloadCompleted;

        public async Task LoadSceneAsync(string sceneName)
        {
            Debug.Log($"[AsyncSceneManager] Loading scene: {sceneName}");
            OnSceneLoadStarted?.Invoke(sceneName);

            try
            {
                var asyncOperation = SceneManager.LoadSceneAsync(sceneName);
                await asyncOperation.ToTask();
                
                Debug.Log($"[AsyncSceneManager] Scene loaded successfully: {sceneName}");
                OnSceneLoadCompleted?.Invoke(sceneName);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[AsyncSceneManager] Failed to load scene {sceneName}: {ex.Message}");
                throw;
            }
        }

        public async Task LoadSceneAdditiveAsync(string sceneName)
        {
            Debug.Log($"[AsyncSceneManager] Loading scene additively: {sceneName}");
            OnSceneLoadStarted?.Invoke(sceneName);

            try
            {
                var asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                await asyncOperation.ToTask();
                
                Debug.Log($"[AsyncSceneManager] Scene loaded additively: {sceneName}");
                OnSceneLoadCompleted?.Invoke(sceneName);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[AsyncSceneManager] Failed to load scene additively {sceneName}: {ex.Message}");
                throw;
            }
        }

        public async Task UnloadSceneAsync(string sceneName)
        {
            Debug.Log($"[AsyncSceneManager] Unloading scene: {sceneName}");
            OnSceneUnloadStarted?.Invoke(sceneName);

            try
            {
                var asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
                await asyncOperation.ToTask();
                
                Debug.Log($"[AsyncSceneManager] Scene unloaded successfully: {sceneName}");
                OnSceneUnloadCompleted?.Invoke(sceneName);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[AsyncSceneManager] Failed to unload scene {sceneName}: {ex.Message}");
                throw;
            }
        }

        public string GetActiveSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }

        public bool IsSceneLoaded(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name == sceneName && scene.isLoaded)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
