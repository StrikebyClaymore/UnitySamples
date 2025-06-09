using Plugins.ServiceLocator;
using UnityEngine;

namespace UISample.Infrastructure
{
    public class CoroutineHelper : MonoBehaviour, IService
    {
        private CoroutineHelper _instance;
        
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(this);
                return;
            }
            DontDestroyOnLoad(this);
        }
    }
}