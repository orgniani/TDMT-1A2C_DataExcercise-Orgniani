using System.Collections;
using UnityEngine;
using DataSources;
using Core.Interactions;

namespace Gameplay
{
    public class LevelManager : MonoBehaviour
    {
        [Header("References")]
        [Header("Data Sources")]
        [SerializeField] private DataSource<PlayerController> playerDataSource;

        [Header("Transforms")]
        [SerializeField] private Transform levelStart;
        
        private PlayerController _playerController;

        private void Awake()
        {
            ValidateReferences();
        }

        private IEnumerator Start()
        {
            while (_playerController == null)
            {
                //TODO: Get reference to player controller from ReferenceManager/DataSource | DONE

                if (playerDataSource.Value != null)
                    _playerController = playerDataSource.Value;

                yield return null;
            }
            _playerController.SetPlayerAtLevelStartAndEnable(levelStart.position);
        }

        private void ValidateReferences()
        {
            if (!playerDataSource)
            {
                Debug.LogError($"{name}: {nameof(playerDataSource)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!levelStart)
            {
                Debug.LogError($"{name}: {nameof(levelStart)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}
