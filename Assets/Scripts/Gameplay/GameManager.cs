using UnityEngine;
using DataSources;
using Scenery;
using Events;
using Core;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private string playId = "Play";
        [SerializeField] private string exitId = "Exit";
        [SerializeField] private string restartId = "Restart";

        [SerializeField] private DataSource<GameManager> gameManagerDataSource;

        //TODO: This should be handled by the Play button, giving the second batch of scenes -SF
        [SerializeField] private SceneryLoadId menuScene;
        [SerializeField] private SceneryLoadId worldScene;
        [SerializeField] private SceneryLoadId[] levels;

        private int _currentLevelIndex = 0;
        private int[] _currentLevelIds;

        public bool IsFinalLevel { get; private set; }

        private void OnEnable()
        {
            if (gameManagerDataSource != null)
                gameManagerDataSource.Value = this;

            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.Win, OnGameOver);
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.Lose, OnGameOver);
            }
        }

        private void Start()
        {
            IsFinalLevel = false;
            InvokeSceneryEvent(menuScene.SceneIndexes);
        }

        private void OnDisable()
        {
            if (gameManagerDataSource != null && gameManagerDataSource.Value == this)
                gameManagerDataSource.Value = null;

            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.Win, OnGameOver);
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.Lose, OnGameOver);
            }
        }

        private void OnGameOver(params object[] args)
        {
            if (!IsFinalLevel)
            {
                NextLevel();
            }
        }

        public void HandleSpecialEvents(string id)
        {
            switch (id)
            {
                case var _ when id == playId || id == restartId:
                    IsFinalLevel = false;

                    _currentLevelIndex = 0;
                    InvokeSceneryEvent(worldScene.SceneIndexes);
                    InvokeSceneryEvent(levels[_currentLevelIndex].SceneIndexes);
                    _currentLevelIds = levels[_currentLevelIndex].SceneIndexes;
                    break;

                case var _ when id == exitId:
                    ExitGame();
                    break;
            }
        }

        private void NextLevel()
        {
            if (_currentLevelIndex < levels.Length - 1)
            {
                _currentLevelIndex++;
                InvokeSceneryEvent(levels[_currentLevelIndex].SceneIndexes);
                _currentLevelIds = levels[_currentLevelIndex].SceneIndexes;
            }
            else
            {
                IsFinalLevel = true;

                EventManager<string>.Instance.InvokeEvent(GameEvents.UnloadScenery, _currentLevelIds);
                EventManager<string>.Instance.InvokeEvent(GameEvents.UnloadScenery, worldScene.SceneIndexes);
            }
        }

        private void InvokeSceneryEvent(int[] sceneIndexes)
        {
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.LoadScenery, sceneIndexes);
        }

        private void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}