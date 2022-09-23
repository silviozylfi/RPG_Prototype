using System;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "New Orient To Target Effect", menuName = "Abilities/Effects/New Orient To Target Effect")]
    public class OrientToTargetEffect : EffectStrategy
    {
        public override void StartEffect(AbilityData abilityData, Action finished)
        {
            abilityData.GetUser().transform.LookAt(abilityData.GetTargetedPoint());
            finished();
        }
    }
}
