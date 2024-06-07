using System;
using UnityEngine;

namespace Gameplay
{
    public class LevelEnd : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            //TODO: Raise event through event system telling the game to show the win sequence.
        }
    }
}
