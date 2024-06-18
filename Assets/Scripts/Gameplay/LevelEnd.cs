using UnityEngine;
using Events;

namespace Gameplay
{
    public class LevelEnd : MonoBehaviour
    {
        [SerializeField] private string winActionName = "Win"; //TODO: SHOULD BE SOMETHING ELSE THAN A STRING MAYBE

        private void OnTriggerEnter(Collider other)
        {
            //TODO: Raise event through event system telling the game to show the win sequence. | DONE

            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(winActionName, true);

            Debug.Log($"{name}: Player touched the flag!");
        }
    }
}
