using UnityEngine;

namespace UISample.Utility
{
    public static class GameObjectExtension
    {
        public static void Show(this GameObject gameObject)
        {
            gameObject.SetActive(true);
        }
        
        public static void Hide(this GameObject gameObject)
        {
            gameObject.SetActive(false);
        }
        
        public static void SetVisible(this GameObject gameObject, bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}