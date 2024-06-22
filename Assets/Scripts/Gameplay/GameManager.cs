using UnityEngine;
using DataSources;
using Scenery;
using Events;
using Core;
using System.Collections.Generic;

namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [Header("References")]
        [Header("Data Sources")]
        [SerializeField] private DataSource<GameManager> gameManagerDataSource;

        [Header("Scenery Ids")]
        [SerializeField] private SceneryLoadId menuScene;
        [SerializeField] private SceneryLoadId worldScene;
        [SerializeField] private SceneryLoadId[] levels;

        private List<SceneryLoadId> _allSceneIds = new List<SceneryLoadId>();

        private int _currentLevelIndex = 0;

        public bool IsFinalLevel { get; private set; }

        private void Awake()
        {
            if (!gameManagerDataSource)
            {
                Debug.LogError($"{name}: {nameof(gameManagerDataSource)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!menuScene)
            {
                Debug.LogError($"{name}: {nameof(menuScene)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!worldScene)
            {
                Debug.LogError($"{name}: {nameof(worldScene)} is null!" +
                               $"\nDisabling ocomponentbject to avoid errors.");
                enabled = false;
                return;
            }

            foreach (SceneryLoadId level in levels)
            {
                if (level == null)
                {
                    Debug.LogError($"{name}: a {nameof(level)} is null!" +
                                   $"\nDisabling component to avoid errors.");
                    enabled = false;
                }
            }

            if (levels.Length <= 0)
            {
                Debug.LogError($"{name}: the array of {nameof(levels)} is empty!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            _allSceneIds.Add(menuScene);
            _allSceneIds.Add(worldScene);

            foreach(var level in levels)
            {
                _allSceneIds.Add(level);
            }
        }

        private void OnEnable()
        {
            if (gameManagerDataSource != null)
                gameManagerDataSource.Value = this;

            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.WinAction, OnGameOver);
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.LoseAction, OnGameOver);
            }
        }

        private void Start()
        {
            IsFinalLevel = false;

            foreach(var id in _allSceneIds)
            {
                if (!id.CanUnload)
                {
                    id.CanUnload = true;
                    InvokeUnloadSceneryEvent(id.SceneIndexes);

                    id.CanUnload = false;

                    continue;
                }

                InvokeUnloadSceneryEvent(id.SceneIndexes);
            }

            _allSceneIds.Clear();
            InvokeLoadSceneryEvent(menuScene.SceneIndexes);
        }

        private void OnDisable()
        {
            if (gameManagerDataSource != null && gameManagerDataSource.Value == this)
                gameManagerDataSource.Value = null;

            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.WinAction, OnGameOver);
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.LoseAction, OnGameOver);
            }
        }

        private void OnGameOver(params object[] args)
        {
            if (!IsFinalLevel)
            {
                NextLevel();
            }
        }

        public void HandlePlayGame()
        {
            IsFinalLevel = false;

            _currentLevelIndex = 0;
            InvokeLoadSceneryEvent(worldScene.SceneIndexes);
            InvokeLoadSceneryEvent(levels[_currentLevelIndex].SceneIndexes);
        }

        private void NextLevel()
        {
            if (_currentLevelIndex < levels.Length - 1)
            {
                _currentLevelIndex++;
                InvokeLoadSceneryEvent(levels[_currentLevelIndex].SceneIndexes);
            }
            else
            {
                IsFinalLevel = true;

                InvokeUnloadSceneryEvent(levels[_currentLevelIndex].SceneIndexes);
                InvokeUnloadSceneryEvent(worldScene.SceneIndexes);

            }
        }

        private void InvokeLoadSceneryEvent(int[] sceneIndexes)
        {
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.LoadScenery, sceneIndexes);
        }

        private void InvokeUnloadSceneryEvent(int[] sceneIndexes)
        {
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.UnloadScenery, sceneIndexes);
        }
    }
}