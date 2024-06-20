using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DataSources;
using Core;
using Events;

namespace Scenery
{
    public class SceneryManager : MonoBehaviour
    {
        [SerializeField] private DataSource<SceneryManager> sceneryManagerDataSource;
        [SerializeField] private SceneryLoadId[] allScenesIds; //TODO: get a new way to avoid unloading the menu -SF
        private int[] _currentLevelIds;

        public event Action OnLoadStart = delegate { };
        /// <summary>
        /// The float given is always between 0 and 1
        /// </summary>
        public event Action<float> OnLoadPercentage = delegate { };
        public event Action OnLoadEnd = delegate { };

        private void OnEnable()
        {
            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.LoadScenery, HandleLoadScenery);
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.UnloadScenery, HandleUnloadScenery);
            }

            if (sceneryManagerDataSource != null)
                sceneryManagerDataSource.Value = this;

            if (EventManager<IId>.Instance)
            {
                foreach (var loadId in allScenesIds)
                {
                    if (loadId == null) continue;
                    EventManager<IId>.Instance.SubscribeToEvent(loadId, HandleLoadScenery);
                }
            }
        }

        private void Start()
        {
            if (allScenesIds.Length > 0 && allScenesIds[0] != null)
            {
                _currentLevelIds = allScenesIds[0].SceneIndexes;
            }
        }

        private void OnDisable()
        {
            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.LoadScenery, HandleLoadScenery);
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.UnloadScenery, HandleUnloadScenery);
            }

            if (sceneryManagerDataSource != null && sceneryManagerDataSource.Value == this)
                sceneryManagerDataSource.Value = null;

            if (EventManager<IId>.Instance)
            {
                foreach (var loadId in allScenesIds)
                {
                    if (loadId == null) continue;
                    EventManager<IId>.Instance.UnsubscribeFromEvent(loadId, HandleLoadScenery);
                }
            }
        }

        private void HandleLoadScenery(params object[] args)
        {
            if (args.Length > 0 && args[0] is int[] newSceneIndexes)
            {
                //TODO: CHECK TO SEE IF THIS COULD BE SIMPLIFIED -SF
                if (_currentLevelIds != null && _currentLevelIds.Length > 0)
                {
                    StartCoroutine(UnloadAndLoadScenes(_currentLevelIds, newSceneIndexes));
                }
                else
                {
                    StartCoroutine(Load(newSceneIndexes, progress => OnLoadPercentage(progress)));
                }

                _currentLevelIds = newSceneIndexes;
            }
        }

        private void HandleUnloadScenery(params object[] args)
        {
            if (args.Length > 0 && args[0] is int[])
            {
                int[] sceneIndexesToUnload = (int[])args[0];

                StartCoroutine(UnloadScenes(sceneIndexesToUnload));
            }
        }

        private IEnumerator UnloadAndLoadScenes(int[] currentSceneIndexes, int[] newSceneIndexes)
        {
            OnLoadStart?.Invoke();
            OnLoadPercentage?.Invoke(0);

            int unloadCount = currentSceneIndexes.Length;
            int loadCount = newSceneIndexes.Length;
            int totalCount = unloadCount + loadCount;

            yield return Unload(currentSceneIndexes, currentIndex => OnLoadPercentage((float)currentIndex / totalCount));

            yield return new WaitForSeconds(2);

            yield return Load(newSceneIndexes, currentIndex => OnLoadPercentage((float)(currentIndex + unloadCount) / totalCount));

            yield return new WaitForSeconds(2);

            _currentLevelIds = newSceneIndexes;

            OnLoadEnd?.Invoke();
        }

        private IEnumerator UnloadScenes(int[] currentSceneIndexes)
        {
            OnLoadStart?.Invoke();
            OnLoadPercentage?.Invoke(0);

            int unloadCount = currentSceneIndexes.Length;

            yield return Unload(currentSceneIndexes, currentIndex => OnLoadPercentage((float)currentIndex / unloadCount));

            yield return new WaitForSeconds(2);

            _currentLevelIds = Array.Empty<int>();

            OnLoadEnd?.Invoke();
        }

        private IEnumerator Load(int[] sceneIndexes, Action<float> onLoadedSceneQtyChanged)
        {
            var current = 0;
            float delayPerScene = 0.5f; // TODO: SERAILIZE -SF

            foreach (var sceneIndex in sceneIndexes)
            {
                var loadOp = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

                if (loadOp == null)
                {
                    Debug.LogError($"Failed to load scene at index {sceneIndex}");
                    continue;
                }

                while (!loadOp.isDone)
                {
                    onLoadedSceneQtyChanged?.Invoke((float)current / sceneIndexes.Length);
                    yield return null;
                }

                yield return new WaitForSeconds(delayPerScene);

                current++;
                onLoadedSceneQtyChanged?.Invoke((float)current / sceneIndexes.Length);
            }
        }

        private IEnumerator Unload(int[] sceneIndexes, Action<float> onLoadedSceneQtyChanged)
        {
            if (sceneIndexes == allScenesIds[0].SceneIndexes) yield break; // avoid unloading the menu 

            var current = 0;
            float delayPerScene = 0.5f; // TODO: SERAILIZE -SF

            foreach (var sceneIndex in sceneIndexes)
            {
                if (SceneManager.GetSceneByBuildIndex(sceneIndex).isLoaded)
                {
                    var unloadOp = SceneManager.UnloadSceneAsync(sceneIndex);

                    if (unloadOp == null)
                    {
                        Debug.LogError($"Failed to unload scene at index {sceneIndex}");
                        continue;
                    }

                    while (!unloadOp.isDone)
                    {
                        onLoadedSceneQtyChanged?.Invoke((float)current / sceneIndexes.Length);
                        yield return null;
                    }

                    yield return new WaitForSeconds(delayPerScene);

                    current++;
                    onLoadedSceneQtyChanged?.Invoke(current);
                }
                else
                {
                    Debug.Log($"Scene at index {sceneIndex} is not currently loaded. Skipping unload operation.");
                }
            }
        }
    }
}