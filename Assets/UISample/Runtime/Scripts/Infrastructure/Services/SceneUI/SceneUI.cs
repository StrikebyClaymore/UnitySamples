using System;
using System.Collections.Generic;
using Plugins.ServiceLocator;
using UISample.UI;
using UnityEngine;

namespace UISample.Infrastructure
{
    public abstract class SceneUI : IService, IInitializable
    {
        protected readonly UIContainer _uiContainer;
        protected readonly Dictionary<Type, BaseController> _controllers = new();
        protected readonly Dictionary<Type, BaseController> _showedControllers = new();
        protected readonly Stack<List<Type>> _previousControllersStack = new();
        public bool Initialized { get; private set; }

        protected SceneUI(UIContainer uiContainer)
        {
            _uiContainer = uiContainer;
        }

        public virtual void Initialize()
        {
            Initialized = true;
        }
        
        public T GetController<T>() where T : BaseController
        {
            foreach (var controller in _controllers.Values)
            {
                if (controller is T tController)
                {
                    return tController;
                }
            }
            Debug.LogError($"Controller {typeof(T)} not found.");
            return null;
        }
        
        public bool ShowController<T>() where T : BaseController
        {
            var type = typeof(T);
            return ShowController(type);
        }
        
        public bool HideController<T>(bool savePrevious = true) where T : BaseController
        {
            var type = typeof(T);
            if (savePrevious)
                SaveCurrentControllers();
            if (_showedControllers.TryGetValue(type, out var controller))
            {
                controller.Hide();
                _showedControllers.Remove(type);
                return true;
            }
            Debug.LogWarning($"Can't hide {type} controller.");
            return false;
        }
        
        public void ShowPreviousControllers()
        {
            if (_previousControllersStack.Count == 0)
                return;
            HideAllControllers(false);
            var previousControllers = _previousControllersStack.Pop();
            foreach (var type in previousControllers)
            {
                ShowController(type);
            }
        }
        
        public void HideAllControllers(bool savePrevious = true, BaseController excluding = null)
        {
            if (savePrevious)
                SaveCurrentControllers();
            foreach (var pair in _showedControllers)
            {
                var controller = pair.Value;
                if(controller == excluding)
                    continue;
                controller.Hide();
            }
            _showedControllers.Clear();
        }
        
        public void ClearControllersStack()
        {
            _previousControllersStack.Clear();
        }
        
        protected abstract void CreateControllers(UIContainer uiContainer);
        
        private bool ShowController(Type type)
        {
            if (_showedControllers.ContainsKey(type))
            {
                Debug.LogWarning($"The {type} controller is already open.");
                return false;
            }
            if (_controllers.TryGetValue(type, out var controller))
            {
                controller.Show();
                _showedControllers.Add(type, controller);
                return true;
            }
            Debug.LogWarning($"Can't show {type} controller.");
            return false;
        }
        
        private void SaveCurrentControllers()
        {
            if (_showedControllers.Count > 0)
            {
                _previousControllersStack.Push(new List<Type>(_showedControllers.Keys));
            }
        }
    }
}