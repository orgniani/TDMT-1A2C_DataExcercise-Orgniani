using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Interactions;

namespace Characters
{
    public class Character : MonoBehaviour, ISteerable, ITarget
    {
        private Vector3 _currentDirection;
        [SerializeField] private float speed;

        private void Update()
        {
            transform.Translate(_currentDirection * (Time.deltaTime * speed), Space.Self);
        }

        public void SetDirection(Vector3 direction)
        {
            _currentDirection = direction;
        }

        public void ReceiveAttack()
        {
            //TODO: Raise event through event system telling the game to show the defeat sequence.
            Debug.Log($"{name}: received an attack!");
        }
    }
}