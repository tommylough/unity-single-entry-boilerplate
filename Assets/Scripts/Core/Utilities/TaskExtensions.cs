using System.Threading.Tasks;
using UnityEngine;

namespace Core.Utilities
{
    /// <summary>
    /// Extension methods for converting Unity async operations to Tasks
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Convert a Unity AsyncOperation to a Task
        /// </summary>
        public static Task ToTask(this AsyncOperation asyncOperation)
        {
            var tcs = new TaskCompletionSource<bool>();

            asyncOperation.completed += _ =>
            {
                tcs.SetResult(true);
            };

            return tcs.Task;
        }

        /// <summary>
        /// Convert a Unity AsyncOperation to a Task with progress reporting
        /// </summary>
        public static Task ToTask(this AsyncOperation asyncOperation, System.IProgress<float> progress)
        {
            var tcs = new TaskCompletionSource<bool>();

            asyncOperation.completed += _ =>
            {
                progress?.Report(1.0f);
                tcs.SetResult(true);
            };

            // Report progress during operation
            ReportProgressAsync(asyncOperation, progress, tcs);

            return tcs.Task;
        }

        private static async void ReportProgressAsync(AsyncOperation asyncOperation, System.IProgress<float> progress, TaskCompletionSource<bool> tcs)
        {
            while (!asyncOperation.isDone && !tcs.Task.IsCompleted)
            {
                progress?.Report(asyncOperation.progress);
                await Task.Yield();
            }
        }
    }
}
