using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scenery
{
    [Serializable]
    public class Level
    {
        //TODO: WATCH SCENERY MANAGEMENT CLASS AND CHANGE THIS -SF
        [field: SerializeField] public List<string> SceneNames { get; private set; }
    }
}
