using UnityEngine;

namespace Pool
{
    public class PoolsRoot : MonoBehaviour
    {
        private static PoolsRoot _instance;
        public static PoolsRoot Instance => _instance ??= new GameObject(nameof(PoolsRoot)).AddComponent<PoolsRoot>();
        
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