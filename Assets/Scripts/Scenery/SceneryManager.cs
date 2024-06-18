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
        [SerializeField] private SceneryLoadId[] levelIds; //This might not be necessary
        private int[] _currentLevelIds;

        public event Action OnLoadStart = delegate { };
        /// <summary>
        /// The float given is always between 0 and 1
        /// </summary>
        public event Action<float> OnLoadPercentage = delegate { };
        public event Action OnLoadEnd = delegate { };

        private void OnEnable()
        {
            if (EventManager<IId>.Instance)
            {
                foreach (var loadId in levelIds)
                {
                    if (loadId == null)
                        continue;
                    EventManager<IId>.Instance.SubscribeToEvent(loadId, HandleLoadScenery);
                    //TODO: IT SHOULD BE DONE BY INVOKE EVENTMANAGER! -SF
                }
            }

            if (sceneryManagerDataSource != null)
                sceneryManagerDataSource.Value = this;
        }

        private void Start()
        {
            foreach (var loadId in levelIds)
            {
                if (loadId == null)
                    continue;
                StartCoroutine(LoadSceneBatch(loadId.SceneIndexes));
            }
        }

        private void OnDisable()
        {
            if (EventManager<IId>.Instance)
            {
                foreach (var loadId in levelIds)
                {
                    if (loadId == null)
                        continue;
                    EventManager<IId>.Instance.UnsubscribeFromEvent(loadId, HandleLoadScenery);
                }
            }

            if (sceneryManagerDataSource != null && sceneryManagerDataSource.Value == this)
                sceneryManagerDataSource.Value = null;
        }

        private void HandleLoadScenery(params object[] levelId)
        {
            if (levelId.Length > 0 && levelId[0] is SceneryLoadId)
            {
                var sceneryLoadId = levelId[0] as SceneryLoadId;
                StartCoroutine(LoadSceneBatch(sceneryLoadId.SceneIndexes));
            }
        }

        public void ChangeLevel(int[] sceneIndexes)
        {
            StartCoroutine(ChangeLevel(_currentLevelIds, sceneIndexes));
        }

        private IEnumerator ChangeLevel(int[] currentSceneIndexes, int[] newSceneIndexes)
        {
            OnLoadStart();
            OnLoadPercentage(0);

            var unloadCount = currentSceneIndexes.Length;
            var loadCount = newSceneIndexes.Length;
            var total = unloadCount + loadCount;

            //TODO: Serialize the waiting seconds -SF
            yield return new WaitForSeconds(2);


            //TODO: Menu scene should not unload but level 1 should -SF
            //yield return Unload(currentSceneIndexes, currentIndex => OnLoadPercentage((float)currentIndex / total));

            yield return new WaitForSeconds(2);

            yield return Load(newSceneIndexes, currentIndex => OnLoadPercentage((float)(currentIndex + unloadCount) / total));

            yield return new WaitForSeconds(2);

            _currentLevelIds = newSceneIndexes;

            OnLoadEnd();
        }

        private IEnumerator LoadSceneBatch(int[] sceneIndexes)
        {
            //TODO: This is a cheating value, do not use in production! -SF
            var addedWeight = 5;

            OnLoadStart();
            OnLoadPercentage(0);

            var total = sceneIndexes.Length + addedWeight;
            var current = 0;

            yield return Load(sceneIndexes,
                currentIndex => OnLoadPercentage(0)); //TODO: I CHANGED THIS -SF

            //TODO: This is cheating so the screen is shown over a lot of time :) -SF
            for (; current <= total; current++)
            {
                yield return new WaitForSeconds(1);
                OnLoadPercentage((float)current / total);
            }

            _currentLevelIds = sceneIndexes;
            OnLoadEnd();
        }

        private IEnumerator Load(int[] sceneIndexes, Action<int> onLoadedSceneQtyChanged)
        {
            var current = 0;

            foreach (var sceneIndex in sceneIndexes)
            {
                //TODO: Null check if load op is null -SF

                var loadOp = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
                yield return new WaitUntil(() => loadOp.isDone);
                current++;
                onLoadedSceneQtyChanged(current);
            }
        }

        private IEnumerator Unload(int[] sceneIndexes, Action<int> onUnloadedSceneQtyChanged)
        {
            var current = 0;
            foreach (var sceneIndex in sceneIndexes)
            {
                var loadOp = SceneManager.UnloadSceneAsync(sceneIndex);
                yield return new WaitUntil(() => loadOp.isDone);
                current++;
                onUnloadedSceneQtyChanged(current);
            }
        }
    }
}
