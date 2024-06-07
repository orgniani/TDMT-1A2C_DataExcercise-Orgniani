using Characters;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Character))]
    public class PlayerController : MonoBehaviour
    {
        private Character _character;

        private void Reset()
        {
            _character = GetComponent<Character>();
        }

        private void Awake()
        {
            _character ??= GetComponent<Character>();
            if (_character)
            {
                _character.enabled = false;
            }
        }

        private void OnEnable()
        {
            //TODO: Subscribe to inputs via event manager/event channel
            //TODO: Set itself as player reference via ReferenceManager/DataSource
        }

        private void OnDisable()
        {
            //TODO: Unsubscribe from all inputs via event manager/event channel
            //TODO: Remove itself as player reference via reference manager/dataSource
        }

        public void SetPlayerAtLevelStartAndEnable(Vector3 levelStartPosition)
        {
            transform.position = levelStartPosition;
            _character.enabled = true;
        }
        
        private void HandleMove(Vector2 direction)
        {
            _character.SetDirection(new Vector3(direction.x, 0, direction.y));
        }

        private void HandleRun(bool shouldRun)
        {
            if (shouldRun)
                _character.StartRunning();
            else
                _character.StopRunning();
        }
    }
}
