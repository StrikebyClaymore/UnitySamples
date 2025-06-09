using System;
using System.Collections;
using System.Collections.Generic;
using Plugins.ServiceLocator;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UISample.Infrastructure
{
    public class SceneLoader : IService
    {
        private readonly MonoBehaviour _coroutineHelper;
        public event Action OnLoadingStart;
        public event Action<float> OnLoadingUpdate;
        public event Action OnLoadingEnd;

        public SceneLoader()
        {
            _coroutineHelper = ServiceLocator.Get<CoroutineHelper>();
        }

        public Coroutine LoadSceneAsync(string sceneName, List<IEnumerator> tasks = null)
        {
            return _coroutineHelper.StartCoroutine(LoadSceneRoutine(CreateLoadAsyncOperation(sceneName), tasks));
        }

        public Coroutine LoadSceneAsync(int buildIndex, List<IEnumerator> tasks = null)
        {
            return _coroutineHelper.StartCoroutine(LoadSceneRoutine(CreateLoadAsyncOperation(buildIndex), tasks));
        }

        private IEnumerator LoadSceneRoutine(AsyncOperation asyncOperation, List<IEnumerator> tasks)
        {
            var tasksCount = 1;
            if (tasks != null)
                tasksCount += tasks.Count;

            float progress = 0f;
            OnLoadingStart?.Invoke();

            while (!asyncOperation.isDone)
            {
                progress = asyncOperation.progress / tasksCount;
                OnLoadingUpdate?.Invoke(progress);
                yield return null;
            }

            progress = 1f / tasksCount;
            OnLoadingUpdate?.Invoke(progress);

            if (tasks != null)
            {
                foreach (var task in tasks)
                {
                    yield return _coroutineHelper.StartCoroutine(task);
                    progress += 1f / tasksCount;
                    OnLoadingUpdate?.Invoke(progress);
                }
            }
            
            OnLoadingEnd?.Invoke();
        }

        private AsyncOperation CreateLoadAsyncOperation(string sceneName) =>
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        private AsyncOperation CreateLoadAsyncOperation(int buildIndex) =>
            SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);

        public Coroutine UnloadSceneAsync(int buildIndex)
        {
            return _coroutineHelper.StartCoroutine(UnloadSceneRoutine(buildIndex));
        }

        private IEnumerator UnloadSceneRoutine(int buildIndex)
        {
            var async = SceneManager.UnloadSceneAsync(buildIndex);
            while (!async.isDone)
                yield return null;
        }
    }
}
