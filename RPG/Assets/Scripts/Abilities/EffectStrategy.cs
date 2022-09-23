using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Abilities
{
    public abstract class EffectStrategy : ScriptableObject
    {
        public abstract void StartEffect(AbilityData abilityData, Action finished);
    }
}
