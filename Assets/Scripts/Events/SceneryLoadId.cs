using UnityEngine;
using Events;
using System.Collections.Generic;

namespace Scenery
{
    [CreateAssetMenu(menuName = "Models/Scenery load ID", fileName = "SceneryLoadId", order = 0)]
    public class SceneryLoadId : ScriptableObject, IId
    {
        [SerializeField] private string logName;
        [field: SerializeField] public int[] SceneIndexes { get; private set; }

        public override string ToString()
        {
            return $"<color=green>{logName}</color> ({base.ToString()})";
        }
    }
}
