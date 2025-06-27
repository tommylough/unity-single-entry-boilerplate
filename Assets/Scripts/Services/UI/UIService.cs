using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Services.UI
{
    /// <summary>
    /// UI service implementation for managing user interface
    /// </summary>
    public class UIService : IUIService
    {
        private Canvas _mainCanvas;
        private CanvasGroup _mainCanvasGroup;
        private Dictionary<string, GameObject> _panels = new Dictionary<string, GameObject>();

        public Canvas MainCanvas => _mainCanvas;

        public async Task InitializeAsync()
        {
            Debug.Log("[UIService] Initialized.");
            await Task.CompletedTask;
        }

        public void SetMainCanvas(Canvas canvas)
        {
            _mainCanvas = canvas;
            _mainCanvasGroup = canvas.GetComponent<CanvasGroup>();
            
            if (_mainCanvasGroup == null)
            {
                _mainCanvasGroup = canvas.gameObject.AddComponent<CanvasGroup>();
            }

            // Find and register all panels in the canvas
            RegisterPanels();
            
            Debug.Log($"[UIService] Main canvas set: {canvas.name}");
        }

        public void ShowPanel(string panelName)
        {
            if (_panels.TryGetValue(panelName, out var panel))
            {
                panel.SetActive(true);
                Debug.Log($"[UIService] Showed panel: {panelName}");
            }
            else
            {
                Debug.LogWarning($"[UIService] Panel not found: {panelName}");
            }
        }

        public void HidePanel(string panelName)
        {
            if (_panels.TryGetValue(panelName, out var panel))
            {
                panel.SetActive(false);
                Debug.Log($"[UIService] Hid panel: {panelName}");
            }
            else
            {
                Debug.LogWarning($"[UIService] Panel not found: {panelName}");
            }
        }

        public void TogglePanel(string panelName)
        {
            if (_panels.TryGetValue(panelName, out var panel))
            {
                panel.SetActive(!panel.activeInHierarchy);
                Debug.Log($"[UIService] Toggled panel: {panelName} - Active: {panel.activeInHierarchy}");
            }
            else
            {
                Debug.LogWarning($"[UIService] Panel not found: {panelName}");
            }
        }

        public void HideAllPanels()
        {
            foreach (var panel in _panels.Values)
            {
                panel.SetActive(false);
            }
            Debug.Log("[UIService] All panels hidden.");
        }

        public bool IsPanelVisible(string panelName)
        {
            if (_panels.TryGetValue(panelName, out var panel))
            {
                return panel.activeInHierarchy;
            }
            return false;
        }

        public void ShowMessage(string message, float duration = 3f)
        {
            // This is a simple implementation - in a real project you'd want a more sophisticated popup system
            Debug.Log($"[UIService] Message: {message}");
            
            // TODO: Implement actual popup UI system
            // For now, just log the message
        }

        public void SetUIInteractable(bool interactable)
        {
            if (_mainCanvasGroup != null)
            {
                _mainCanvasGroup.interactable = interactable;
                _mainCanvasGroup.blocksRaycasts = interactable;
                Debug.Log($"[UIService] UI interactable set to: {interactable}");
            }
        }

        private void RegisterPanels()
        {
            if (_mainCanvas == null) return;

            _panels.Clear();

            // Find all direct children that could be panels
            for (int i = 0; i < _mainCanvas.transform.childCount; i++)
            {
                var child = _mainCanvas.transform.GetChild(i);
                var panelName = child.name;
                
                // Register the panel
                _panels[panelName] = child.gameObject;
            }

            Debug.Log($"[UIService] Registered {_panels.Count} panels.");
        }
    }
}
