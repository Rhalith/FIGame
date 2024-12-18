using Scripts.EventBus;
using Scripts.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Managers
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private Transform _menuPanel;
        
        private Transform _currentPanel;

        private EventSystem _eventSystem;
        private GameObject _selectedObject;

        private bool _isUsingKeyboardOrGamepad;
        
        private bool _canSelect = true;

        private void OnEnable()
        {
            EventBus<CheckSelectableElementEvent>.AddListener(ChangeSelectionState);
        }
        
        private void OnDisable()
        {
            EventBus<CheckSelectableElementEvent>.RemoveListener(ChangeSelectionState);
        }

        private void ChangeSelectionState(object sender, CheckSelectableElementEvent @event)
        {
            _canSelect = @event.CanSelect;
        }

        private void Start()
        {
            _eventSystem = EventSystem.current;
            _currentPanel = _menuPanel;
            SetFirstSelectable();
            Cursor.visible = false;
        }

        private void Update()
        {
            if (!_canSelect) return;
            if(_isUsingKeyboardOrGamepad) HandleDeselection();
            DetectInputSource();
        }

        
        public void ChangePanel(Transform panel)
        {
            _currentPanel = panel;
            SetFirstSelectable();
        }
        private void SetFirstSelectable()
        {
            _selectedObject = FindFirstSelectable(_currentPanel);

            if (_selectedObject != null)
            {
                _eventSystem.SetSelectedGameObject(_selectedObject);
                Debug.Log($"First selected object: {_selectedObject.name}");
            }
            else
            {
                Debug.LogWarning("No selectable UI element found in the menu panel.");
            }
        }

        private void HandleDeselection()
        {
            // If no UI element is selected or the current selection is inactive
            if (!_eventSystem.currentSelectedGameObject || 
                !_eventSystem.currentSelectedGameObject.activeInHierarchy)
            {
                SetFirstSelectable();
            }
        }

        private void DetectInputSource()
        {
            // Detect keyboard/gamepad input
            if (Input.anyKeyDown || Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                if (!_isUsingKeyboardOrGamepad)
                {
                    _isUsingKeyboardOrGamepad = true;
                    Cursor.visible = false; // Hide cursor
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }

            // Detect mouse movement or click
            if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0 || Input.GetMouseButtonDown(0))
            {
                if (_isUsingKeyboardOrGamepad)
                {
                    _isUsingKeyboardOrGamepad = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true; // Show cursor
                    _selectedObject = null;
                    _eventSystem.SetSelectedGameObject(_selectedObject); // Deselect UI object
                }
            }
        }

        private GameObject FindFirstSelectable(Transform parent)
        {
            // Find the first selectable UI element
            foreach (Transform child in parent)
            {
                if (!child.gameObject.activeInHierarchy) continue;

                if (child.TryGetComponent<UnityEngine.UI.Selectable>(out var selectable))
                {
                    if(selectable.interactable) return child.gameObject;
                }
            }

            // Recursively search deeper children
            foreach (Transform child in parent)
            {
                if (!child.gameObject.activeInHierarchy) continue;

                GameObject selectableInChild = FindFirstSelectable(child);
                if (selectableInChild != null)
                {
                    return selectableInChild;
                }
            }

            return null;
        }
    }
}
