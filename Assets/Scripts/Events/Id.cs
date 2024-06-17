using System;
using UnityEngine;

namespace Events
{
    [Serializable]
    public class Id : IId
    {
        [field: SerializeField] public string name { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
