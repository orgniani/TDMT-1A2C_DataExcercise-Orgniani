using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DataSources;
using Events;

namespace Scenery
{
    public class SceneryManager : MonoBehaviour
    {
        [SerializeField] private DataSource<SceneryManager> sceneryManagerDataSource;
        [SerializeField] private SceneryLoadId[] levelIds;
        private int[] _currentLevelIds;

        public event Action OnLoadStart = delegate { };
        /// <summary>
        /// The float given is always between 0 and 1
        /// </summary>
        public event Action<float> OnLoadPercentage = delegate { };
        public event Action OnLoadEnd = delegate { };

        [SerializeField] private string loseActionName = "Lose";
        [SerializeField] private string winActionName = "Win";

        private int _currentLevelIndex = 0;


        private void OnEnable()
        {
            if (EventManager<IId>.Instance)
            {
                foreach (var loadId in levelIds)
                {
                    if (loadId == null) continue;
                    EventManager<IId>.Instance.SubscribeToEvent(loadId, HandleLoadScenery);
                    //TODO: IT SHOULD BE DONE BY INVOKE EVENTMANAGER! -SF
                }
            }

            if (sceneryManagerDataSource != null)
                sceneryManagerDataSource.Value = this;

            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.SubscribeToEvent(winActionName, HandleLoadScenery);
                EventManager<string>.Instance.SubscribeToEvent(loseActionName, HandleLoadScenery);
            }
        }

        private void Start()
        {
            if (levelIds.Length == 0 || levelIds[0] == null) return;
            _currentLevelIds = levelIds[0].SceneIndexes;
            ChangeLevel(levelIds[0].SceneIndexes);
        }

        private void OnDisable()
        {
            if (EventManager<IId>.Instance)
            {
                foreach (var loadId in levelIds)
                {
                    if (loadId == null) continue;
                    EventManager<IId>.Instance.UnsubscribeFromEvent(loadId, HandleLoadScenery);
                }
            }

            if (sceneryManagerDataSource != null && sceneryManagerDataSource.Value == this)
                sceneryManagerDataSource.Value = null;

            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.UnsubscribeFromEvent(winActionName, HandleLoadScenery);
                EventManager<string>.Instance.UnsubscribeFromEvent(loseActionName, HandleLoadScenery);
            }
        }

        private void HandleLoadScenery(params object[] levelId)
        {
            if (levelId.Length > 0 && levelId[0] is int[] id)
            {
                if (_currentLevelIndex < levelIds.Length - 1)
                {
                    ChangeLevel(id);
                    _currentLevelIndex++;
                }
                else
                {
                    ChangeLevel(id, isFinalLevel: true); // Pass a flag indicating it's the final level
                }
            }

        }

        private void ChangeLevel(int[] sceneIndexes, bool isFinalLevel = false)
        {
            StartCoroutine(UnloadAndLoadScenes(_currentLevelIds, sceneIndexes, isFinalLevel));
        }

        private IEnumerator UnloadAndLoadScenes(int[] currentSceneIndexes, int[] newSceneIndexes, bool isFinalLevel)
        {
            OnLoadStart();
            OnLoadPercentage(0);

            var unloadCount = currentSceneIndexes.Length;
            var loadCount = newSceneIndexes.Length;
            var total = unloadCount + loadCount;

            yield return new WaitForSeconds(2);

            yield return Unload(currentSceneIndexes, currentIndex => OnLoadPercentage((float)currentIndex / total));

            if (!isFinalLevel)
            {
                yield return new WaitForSeconds(2);

                yield return Load(newSceneIndexes, currentIndex => OnLoadPercentage((float)(currentIndex + unloadCount) / total));


                _currentLevelIds = newSceneIndexes;
            }

            else
            {
                _currentLevelIds = levelIds[0].SceneIndexes;
            }

            yield return new WaitForSeconds(2);


            OnLoadEnd();
        }

        private IEnumerator Load(int[] sceneIndexes, Action<int> onLoadedSceneQtyChanged)
        {
            var current = 0;

            foreach (var sceneIndex in sceneIndexes)
            {
                var loadOp = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

                if (loadOp == null)
                {
                    Debug.LogError($"Failed to load scene at index {sceneIndex}");
                    continue;
                }

                yield return new WaitUntil(() => loadOp.isDone);
                current++;
                onLoadedSceneQtyChanged(current);
            }
        }

        private IEnumerator Unload(int[] sceneIndexes, Action<int> onLoadedSceneQtyChanged)
        {
            if (sceneIndexes == levelIds[0].SceneIndexes) yield break;

            var current = 0;

            foreach (var sceneIndex in sceneIndexes)
            {
                var unloadOp = SceneManager.UnloadSceneAsync(sceneIndex);

                if (unloadOp == null)
                {
                    Debug.LogError($"Failed to unload scene at index {sceneIndex}");
                    continue;
                }

                yield return new WaitUntil(() => unloadOp.isDone);
                current++;
                onLoadedSceneQtyChanged(current);
            }
        }
    }
}
