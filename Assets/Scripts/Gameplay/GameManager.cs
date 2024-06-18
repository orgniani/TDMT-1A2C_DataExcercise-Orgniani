using UnityEngine;
using DataSources;
using Scenery;

namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private string playId = "Play";
        [SerializeField] private string exitId = "Exit";

        [SerializeField] private DataSource<GameManager> gameManagerDataSource;
        [SerializeField] private DataSource<SceneryManager> sceneryManagerDataSource;

        //TODO: This should be handled by the Play button, giving the second batch of scenes -SF
        [SerializeField] private int[] firstScenesToLoad;
        //[SerializeField] private SceneryLoadId[] secondScenesToLoad;
        //[SerializeField] private SceneryLoadId[] thirdScenesToLoad;

        private void OnEnable()
        {
            if (gameManagerDataSource != null)
                gameManagerDataSource.Value = this;
        }

        private void OnDisable()
        {
            if (gameManagerDataSource != null && gameManagerDataSource.Value == this)
            {
                gameManagerDataSource.Value = null;
            }
        }

        public void HandleSpecialEvents(string id)
        {
            if (id == playId)
            {
                Debug.Log("PLAYER WANTS TO PLAY!");

                //TODO: Start game logic - SF
                if (sceneryManagerDataSource != null && sceneryManagerDataSource.Value != null)
                {
                    sceneryManagerDataSource.Value.ChangeLevel(firstScenesToLoad);
                }
            }
            else if (id == exitId)
            {

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
            }
        }
    }
}