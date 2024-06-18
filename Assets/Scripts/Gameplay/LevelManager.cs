using System.Collections;
using UnityEngine;
using DataSources;

namespace Gameplay
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Transform levelStart;
        [SerializeField] private DataSource<PlayerController> playerDataSource;
        
        private PlayerController _playerController;

        private IEnumerator Start()
        {
            while (_playerController == null)
            {
                //TODO: Get reference to player controller from ReferenceManager/DataSource | DONE

                if (playerDataSource != null && playerDataSource.Value != null)
                    _playerController = playerDataSource.Value;

                yield return null;
            }
            _playerController.SetPlayerAtLevelStartAndEnable(levelStart.position);
        }
    }
}
