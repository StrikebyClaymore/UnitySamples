using System.Collections;
using Plugins.ServiceLocator;
using UISample.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UISample.Infrastructure
{
    public class EntryPoint
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {
            var coroutineHelper = new GameObject(nameof(CoroutineHelper)).AddComponent<CoroutineHelper>();
            ServiceLocator.Register<CoroutineHelper>(coroutineHelper);
            coroutineHelper.StartCoroutine(InitializeAsync());
        }

        private static IEnumerator InitializeAsync()
        {
            if (SceneManager.GetActiveScene().buildIndex == GameConstants.MainMenuSceneIndex || SceneManager.GetActiveScene().buildIndex == GameConstants.GameplaySceneIndex)
            {
                var asyncOperation = SceneManager.LoadSceneAsync(GameConstants.LoadingSceneIndex);
                while (!asyncOperation.isDone)
                    yield return null;
            }
            if (SceneManager.GetActiveScene().buildIndex == GameConstants.LoadingSceneIndex)
            {
                var appInstaller = GameObject.FindObjectOfType<ApplicationInstaller>();
                appInstaller.Install();
                appInstaller.Initialize();
            }
        }
    }
}