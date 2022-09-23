using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Abilities
{
    public abstract class FilterStrategy : ScriptableObject
    {
        public abstract IEnumerable<GameObject> Filter(IEnumerable<GameObject>objectsToFilter);
    }
}
