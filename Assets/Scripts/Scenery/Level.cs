using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scenery
{
    [Serializable]
    public class Level
    {
        //TODO: Change SceneNames for something better (ids maybe??) - SF
        [field: SerializeField] public List<string> SceneNames { get; private set; }
    }
}
