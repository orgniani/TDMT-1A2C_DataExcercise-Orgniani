using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scenery
{
    [Serializable]
    public class Level
    {
        [field: SerializeField] public List<string> SceneNames { get; private set; }
    }
}
