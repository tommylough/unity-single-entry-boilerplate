using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Services.SaveLoad
{
    /// <summary>
    /// JSON-based save/load service implementation
    /// </summary>
    public class JsonSaveLoadService : ISaveLoadService
    {
        private string _saveDirectory;
        private const string SaveFileExtension = ".json";

        public async Task InitializeAsync()
        {
            _saveDirectory = Path.Combine(Application.persistentDataPath, "SaveData");
            
            // Create save directory if it doesn't exist
            if (!Directory.Exists(_saveDirectory))
            {
                Directory.CreateDirectory(_saveDirectory);
            }

            Debug.Log($"[JsonSaveLoadService] Initialized. Save directory: {_saveDirectory}");
            await Task.CompletedTask;
        }

        public async Task SaveAsync<T>(string key, T data)
        {
            try
            {
                var filePath = GetFilePath(key);
                var json = JsonUtility.ToJson(data, true);
                
                await File.WriteAllTextAsync(filePath, json);
                
                Debug.Log($"[JsonSaveLoadService] Saved data for key: {key}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JsonSaveLoadService] Failed to save data for key {key}: {ex.Message}");
                throw;
            }
        }

        public async Task<T> LoadAsync<T>(string key)
        {
            try
            {
                var filePath = GetFilePath(key);
                
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"Save file not found for key: {key}");
                }

                var json = await File.ReadAllTextAsync(filePath);
                var data = JsonUtility.FromJson<T>(json);
                
                Debug.Log($"[JsonSaveLoadService] Loaded data for key: {key}");
                return data;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JsonSaveLoadService] Failed to load data for key {key}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ExistsAsync(string key)
        {
            var filePath = GetFilePath(key);
            var exists = File.Exists(filePath);
            
            await Task.CompletedTask;
            return exists;
        }

        public async Task DeleteAsync(string key)
        {
            try
            {
                var filePath = GetFilePath(key);
                
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Debug.Log($"[JsonSaveLoadService] Deleted data for key: {key}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JsonSaveLoadService] Failed to delete data for key {key}: {ex.Message}");
                throw;
            }
            
            await Task.CompletedTask;
        }

        public async Task ClearAllAsync()
        {
            try
            {
                if (Directory.Exists(_saveDirectory))
                {
                    var files = Directory.GetFiles(_saveDirectory, $"*{SaveFileExtension}");
                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }
                    
                    Debug.Log("[JsonSaveLoadService] Cleared all save data.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JsonSaveLoadService] Failed to clear all data: {ex.Message}");
                throw;
            }
            
            await Task.CompletedTask;
        }

        public async Task<string[]> GetAllKeysAsync()
        {
            try
            {
                if (!Directory.Exists(_saveDirectory))
                {
                    return new string[0];
                }

                var files = Directory.GetFiles(_saveDirectory, $"*{SaveFileExtension}");
                var keys = new List<string>();
                
                foreach (var file in files)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    keys.Add(fileName);
                }
                
                await Task.CompletedTask;
                return keys.ToArray();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JsonSaveLoadService] Failed to get all keys: {ex.Message}");
                throw;
            }
        }

        private string GetFilePath(string key)
        {
            var sanitizedKey = SanitizeFileName(key);
            return Path.Combine(_saveDirectory, sanitizedKey + SaveFileExtension);
        }

        private string SanitizeFileName(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            foreach (var invalidChar in invalidChars)
            {
                fileName = fileName.Replace(invalidChar, '_');
            }
            return fileName;
        }
    }
}
