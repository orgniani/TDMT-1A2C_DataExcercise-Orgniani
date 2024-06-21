using UnityEngine;
using DataSources;
using Scenery;
using Events;
using Core;

namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private DataSource<GameManager> gameManagerDataSource;

        [SerializeField] private SceneryLoadId menuScene;
        [SerializeField] private SceneryLoadId worldScene;
        [SerializeField] private SceneryLoadId[] levels;

        private int _currentLevelIndex = 0;

        public bool IsFinalLevel { get; private set; }

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

            menuScene.CanUnload = true; //TODO: Get this to look better -SF
            InvokeUnloadSceneryEvent(menuScene.SceneIndexes);
            menuScene.CanUnload = false;

            InvokeUnloadSceneryEvent(worldScene.SceneIndexes);

            foreach(var level in levels)
            {
                InvokeUnloadSceneryEvent(level.SceneIndexes);
            }

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