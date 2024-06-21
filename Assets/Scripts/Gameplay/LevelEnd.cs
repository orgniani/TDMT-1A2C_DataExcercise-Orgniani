using UnityEngine;
using Events;
using Core;

namespace Gameplay
{
    public class LevelEnd : MonoBehaviour
    {
        [SerializeField] private LayerMask playerLayer;

        private void OnTriggerEnter(Collider other)
        {
            //TODO: Raise event through event system telling the game to show the win sequence. | DONE

            if (((1 << other.gameObject.layer) & playerLayer.value) != 0)
            {
                if (EventManager<string>.Instance)
                    EventManager<string>.Instance.InvokeEvent(GameEvents.WinAction, true);

                Debug.Log($"{name}: Player touched the flag!");
            }
        }
    }
}
