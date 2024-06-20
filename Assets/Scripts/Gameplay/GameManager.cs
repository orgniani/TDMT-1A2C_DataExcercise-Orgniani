using UnityEngine;
using DataSources;
using Scenery;
using Events;

namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private string playId = "Play";
        [SerializeField] private string exitId = "Exit";
        [SerializeField] private string restartId = "Restart";

        [SerializeField] private DataSource<GameManager> gameManagerDataSource;

        //TODO: This should be handled by the Play button, giving the second batch of scenes -SF

        [SerializeField] private SceneryLoadId[] levelsToLoad;

        [SerializeField] private string loseActionName = "Lose";
        [SerializeField] private string winActionName = "Win";

        private int _currentLevelIndex = 0;

        public bool IsFinalLevel { get; private set; }

        private void OnEnable()
        {
            IsFinalLevel = false;

            if (gameManagerDataSource != null)
                gameManagerDataSource.Value = this;

            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.SubscribeToEvent(winActionName, HandleChangeLevel);
                EventManager<string>.Instance.SubscribeToEvent(loseActionName, HandleChangeLevel);
            }
        }

        private void OnDisable()
        {
            if (gameManagerDataSource != null && gameManagerDataSource.Value == this)
                gameManagerDataSource.Value = null;

            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.UnsubscribeFromEvent(winActionName, HandleChangeLevel);
                EventManager<string>.Instance.UnsubscribeFromEvent(loseActionName, HandleChangeLevel);
            }
        }

        private void HandleChangeLevel(params object[] args)
        {
            if (args.Length > 0 && args[0] is bool)
            {
                if (_currentLevelIndex < levelsToLoad.Length)
                {
                    _currentLevelIndex++;
                    InvokeSceneryEvent(_currentLevelIndex);
                }

                else
                {
                    InvokeSceneryEvent(1);
                    IsFinalLevel = true;
                    // Optionally, you can invoke some event or method to handle end-of-game logic here
                }
            }
        }

        public void HandleSpecialEvents(string id)
        {
            switch (id)
            {
                case var _ when id == playId || id == restartId:
                    InvokeSceneryEvent(0);
                    break;
                case var _ when id == exitId:
                    ExitGame();
                    break;
            }
        }

        private void InvokeSceneryEvent(int levelBundleIndex)
        {
            if (EventManager<IId>.Instance && levelsToLoad.Length > levelBundleIndex && levelsToLoad[levelBundleIndex] != null)
            {
                EventManager<IId>.Instance.InvokeEvent(levelsToLoad[levelBundleIndex], levelsToLoad[levelBundleIndex].SceneIndexes);
            }
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