using UnityEngine;
using Core.Interactions;
using Core;
using Events;

namespace Characters
{
    public class Character : MonoBehaviour, ISteerable, ITarget
    {
        [Header("Parameters")]
        [Header("Speed")]
        [SerializeField] private float speed = 2.5f;
        [SerializeField] private float runningSpeed = 5;

        [Header("Logs")]
        [SerializeField] private bool enableLogs = true;

        private Vector3 _currentDirection = Vector3.zero;
        private bool _isRunning = false;


        private void Update()
        {
            var currentSpeed = _isRunning ? runningSpeed : speed;
            transform.Translate(_currentDirection * (Time.deltaTime * currentSpeed), Space.World);
        }

        public void SetDirection(Vector3 direction)
        {
            _currentDirection = direction;
        }

        public void StartRunning() => _isRunning = true;

        public void StopRunning() => _isRunning = false;

        public void ReceiveAttack()
        {
            //TODO: Raise event through event system telling the game to show the defeat sequence. | DONE

            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.LoseAction, true);

            if (enableLogs) Debug.Log($"<color=red> {name}: received an attack! </color>");
            Destroy(gameObject);
        }
    }
}