using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DataSources;

namespace Scenery
{
    public class SceneryManager : MonoBehaviour
    {
        [SerializeField] private DataSource<SceneryManager> sceneryManagerDataSource;
        [SerializeField] private Level defaultLevel;
        private Level _currentLevel;

        public event Action OnLoadStart = delegate { };
        /// <summary>
        /// The float given is always between 0 and 1
        /// </summary>
        public event Action<float> OnLoadPercentage = delegate { };
        public event Action OnLoadEnd = delegate { };

        private void OnEnable()
        {
            if (sceneryManagerDataSource != null)
                sceneryManagerDataSource.Value = this;
        }

        private void Start()
        {
            //TODO: Load default level -SF
            StartCoroutine(LoadFirstLevel(defaultLevel));
        }

        private void OnDisable()
        {
            if (sceneryManagerDataSource != null && sceneryManagerDataSource.Value == this)
                sceneryManagerDataSource.Value = null;
        }

        public void ChangeLevel(Level level)
        {
            StartCoroutine(ChangeLevel(_currentLevel, level));
        }

        private IEnumerator ChangeLevel(Level currentLevel, Level newLevel)
        {
            OnLoadStart();
            OnLoadPercentage(0);

            var unloadCount = currentLevel.SceneNames.Count;
            var loadCount = newLevel.SceneNames.Count;
            var total = unloadCount + loadCount;

            //TODO: Serialize the waiting seconds -SF
            yield return new WaitForSeconds(2);


            //TODO: Uncommnet this, Menu scene should not unload but level 1 should -SF
            //yield return Unload(currentLevel, currentIndex => OnLoadPercentage((float)currentIndex / total));

            yield return new WaitForSeconds(2);

            yield return Load(newLevel, currentIndex => OnLoadPercentage((float)(currentIndex + unloadCount) / total));

            yield return new WaitForSeconds(2);

            _currentLevel = newLevel;
            OnLoadEnd();
        }

        private IEnumerator LoadFirstLevel(Level level)
        {
            //TODO: This is a cheating value, do not use in production! -SF
            var addedWeight = 5;

            OnLoadStart();
            OnLoadPercentage(0);
            var total = level.SceneNames.Count + addedWeight;
            var current = 0;
            yield return Load(level,
                currentIndex => OnLoadPercentage((float)currentIndex / total));

            //TODO: This is cheating so the screen is shown over a lot of time :) -SF
            for (; current <= total; current++)
            {
                yield return new WaitForSeconds(1);
                OnLoadPercentage((float)current / total);
            }
            _currentLevel = level;
            OnLoadEnd();
        }

        private IEnumerator Load(Level level, Action<int> onLoadedSceneQtyChanged)
        {
            var current = 0;
            foreach (var sceneName in level.SceneNames)
            {
                //TODO: Null check if load op is null -SF

                var loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                yield return new WaitUntil(() => loadOp.isDone);
                current++;
                onLoadedSceneQtyChanged(current);
            }
        }

        private IEnumerator Unload(Level level, Action<int> onUnloadedSceneQtyChanged)
        {
            var current = 0;
            foreach (var sceneName in level.SceneNames)
            {
                var loadOp = SceneManager.UnloadSceneAsync(sceneName);
                yield return new WaitUntil(() => loadOp.isDone);
                current++;
                onUnloadedSceneQtyChanged(current);
            }
        }
    }
}
