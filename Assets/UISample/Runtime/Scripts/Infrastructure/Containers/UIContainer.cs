using System;
using System.Collections.Generic;
using System.Linq;
using UISample.UI;
using UnityEngine;
#if UNITY_EDITOR
using NaughtyAttributes;
#endif

namespace UISample.Infrastructure
{
    public class UIContainer : MonoBehaviour
    {
        [SerializeField] private List<BaseView> _views;
        
        public T GetView<T>() where T : BaseView
        {
            Type type = typeof(T);
            foreach (var view in _views)
            {
                if (view is T findView)
                    return findView;
            }
            Debug.LogError(new KeyNotFoundException($"View {type} not found."));
            return null;
        }
        
#if UNITY_EDITOR
        [Button]
        public void FindUIViews()
        {
            _views = GetComponentsInChildren<BaseView>(true).ToList();
        }
#endif
    }
}