using UnityEngine;
using Scenery;
using Events;

namespace UI
{
    //TODO: DELETE THIS -SF
    [CreateAssetMenu(menuName = "Config/Button", fileName = "ButtonConfig", order = 0)]
    public class ButtonConfig : ScriptableObject
    {
        [SerializeField] private SceneryLoadId id;
        [field: SerializeField] public string Label { get; private set; }


        public IId Id => id;
    }
}
