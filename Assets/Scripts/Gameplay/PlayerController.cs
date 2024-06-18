using Characters;
using Core.Interactions;
using DataSources;
using Events;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Character))]
    public class PlayerController : MonoBehaviour, ITarget
    {
        private Character _character;

        [SerializeField] private DataSource<PlayerController> playerDataSource;
        [SerializeField] private string moveActionName = "Move";
        [SerializeField] private string runActionName = "Run";

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

            if (EventManager<string>.Instance)
                EventManager<string>.Instance.SubscribeToEvent(moveActionName, OnMoveEvent);
            
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.SubscribeToEvent(runActionName, OnRunEvent);

            //TODO: Set itself as player reference via ReferenceManager/DataSource | DONE
            if (playerDataSource != null)
                playerDataSource.Value = this;
        }

        private void OnDisable()
        {
            //TODO: Unsubscribe from all inputs via event manager/event channel

            if (EventManager<string>.Instance)
                EventManager<string>.Instance.UnsubscribeFromEvent(moveActionName, OnMoveEvent);
            
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.UnsubscribeFromEvent(runActionName, OnRunEvent);


            //TODO: Remove itself as player reference via reference manager/dataSource | DONE
            if (playerDataSource != null)
                playerDataSource.Value = null;
        }

        public void SetPlayerAtLevelStartAndEnable(Vector3 levelStartPosition)
        {
            transform.position = levelStartPosition;
            _character.enabled = true;
        }
       
        private void OnMoveEvent(params object[] args)
        {
            if(args.Length > 0 && args[0] is Vector2 direction)
            {
                HandleMove(direction);
            }
        }

        private void OnRunEvent(params object[] args)
        {
            if (args.Length > 0 && args[0] is bool shouldRun)
            {
                HandleRun(shouldRun);
            }
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

        public void ReceiveAttack()
        {

        }
    }
}
