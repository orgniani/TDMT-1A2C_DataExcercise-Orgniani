using System.Collections;
using UnityEngine;

namespace Gameplay
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Transform levelStart;
        
        private PlayerController _playerController;
        private IEnumerator Start()
        {
            while (_playerController == null)
            {
                //TODO: Get reference to player controller from ReferenceManager/DataSource
                yield return null;
            }
            _playerController.SetPlayerAtLevelStartAndEnable(levelStart.position);
        }
    }
}
