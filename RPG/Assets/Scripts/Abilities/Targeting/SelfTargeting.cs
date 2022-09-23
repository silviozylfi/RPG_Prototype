using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using System;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Self Targeting", menuName = "Abilities/Targeting/Self Targeting", order = 0)]
    public class SelfTargeting : TargetingStrategy
    {
        public override void StartTargeting(AbilityData abilityData, Action finished)
        {
            abilityData.SetTargets(new GameObject[] { abilityData.GetUser() });
            abilityData.SetTargetedPoint(abilityData.GetUser().transform.position);
            finished();
        }
    }
}
