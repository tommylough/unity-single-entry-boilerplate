using System.Threading.Tasks;

namespace Services.SaveLoad
{
    /// <summary>
    /// Interface for save/load operations
    /// </summary>
    public interface ISaveLoadService
    {
        /// <summary>
        /// Save data to persistent storage
        /// </summary>
        Task SaveAsync<T>(string key, T data);

        /// <summary>
        /// Load data from persistent storage
        /// </summary>
        Task<T> LoadAsync<T>(string key);

        /// <summary>
        /// Check if data exists for a key
        /// </summary>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// Delete saved data for a key
        /// </summary>
        Task DeleteAsync(string key);

        /// <summary>
        /// Clear all saved data
        /// </summary>
        Task ClearAllAsync();

        /// <summary>
        /// Get all available save keys
        /// </summary>
        Task<string[]> GetAllKeysAsync();
    }
}
