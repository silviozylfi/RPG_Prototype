using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Demo Targeting", menuName = "Abilities/Targeting/Demo", order = 0)]
    public class DemoTargeting : TargetingStrategy
    {
        public override void StartTargeting(AbilityData abilityData, Action finished)
        {
            finished();
        }
    }
}
