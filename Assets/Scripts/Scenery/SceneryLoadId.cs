using UnityEngine;
using Events;

namespace Scenery
{
    [CreateAssetMenu(menuName = "Models/Scenery load ID", fileName = "SceneryLoadId", order = 0)]
    public class SceneryLoadId : ScriptableObject, IId
    {
        [SerializeField] private string logName;
        [field: SerializeField] public int[] SceneIndexes { get; private set; }
        [field: SerializeField] public bool CanUnload { get; set; } = true;

        public override string ToString()
        {
            return $"<color=green>{logName}</color> ({base.ToString()})";
        }
    }

}