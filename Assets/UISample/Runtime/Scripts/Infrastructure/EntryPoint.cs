using UnityEngine;

namespace UISample.Infrastructure
{
    public class EntryPoint : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {
            var installer = FindObjectOfType<MainSceneInstaller>();
            installer.Install();
            installer.Initialize();
        }
    }
}