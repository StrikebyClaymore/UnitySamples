using UnityEngine;

namespace UISample.Utility
{
    public static class ComponentExtension
    {
        public static void Show(this Component component)
        {
            component.gameObject.SetActive(true);
        }
        
        public static void Hide(this Component component)
        {
            component.gameObject.SetActive(false);
        }
        
        
        public static void SetActive(this Component component, bool active)
        {
            component.gameObject.SetActive(active);
        }
    }
}