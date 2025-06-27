using UnityEngine;

namespace Services.UI
{
    /// <summary>
    /// Interface for UI management service
    /// </summary>
    public interface IUIService
    {
        /// <summary>
        /// Get the main UI canvas
        /// </summary>
        Canvas MainCanvas { get; }

        /// <summary>
        /// Show a UI panel by name
        /// </summary>
        void ShowPanel(string panelName);

        /// <summary>
        /// Hide a UI panel by name
        /// </summary>
        void HidePanel(string panelName);

        /// <summary>
        /// Toggle a UI panel by name
        /// </summary>
        void TogglePanel(string panelName);

        /// <summary>
        /// Hide all UI panels
        /// </summary>
        void HideAllPanels();

        /// <summary>
        /// Check if a panel is currently visible
        /// </summary>
        bool IsPanelVisible(string panelName);

        /// <summary>
        /// Create a popup message
        /// </summary>
        void ShowMessage(string message, float duration = 3f);

        /// <summary>
        /// Set UI interactable state
        /// </summary>
        void SetUIInteractable(bool interactable);
    }
}
