using UnityEngine;
using Events;
using System.Linq;

namespace Scenery
{
    [CreateAssetMenu(menuName = "Models/Scenery load ID", fileName = "SceneryLoadId", order = 0)]
    public class SceneryLoadId : ScriptableObject, IId
    {
        [SerializeField] private string logName;
        [field: SerializeField] public int[] SceneIndexes { get; private set; }
        [field: SerializeField] public bool CanUnload { get; set; } = true;

        private void OnValidate()
        {
            if (SceneIndexes == null || SceneIndexes.Length == 0)
            {
                Debug.LogError($"{logName}: the array of {nameof(SceneIndexes)} is empty!");
                return;
            }

            var duplicates = SceneIndexes
                .GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(y => y.Key)
                .ToList();

            if (duplicates.Count > 0)
            {
                Debug.LogError($"{logName}: Duplicate scene indexes found: {string.Join(", ", duplicates)}");
            }
        }
    }
}