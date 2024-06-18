using UnityEngine;
using Core.Interactions;
using Events;

namespace Characters
{
    public class Character : MonoBehaviour, ISteerable, ITarget
    {
        [SerializeField] private float speed = 2.5f;
        [SerializeField] private float runningSpeed = 5;

        [SerializeField] private string loseActionName = "Lose"; //TODO: SHOULD BE SOMETHING ELSE THAN A STRING MAYBE, maybe all actions names should be in a setup class -SF

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
                EventManager<string>.Instance.InvokeEvent(loseActionName, true);

            Debug.Log($"{name}: received an attack!");
        }
    }
}