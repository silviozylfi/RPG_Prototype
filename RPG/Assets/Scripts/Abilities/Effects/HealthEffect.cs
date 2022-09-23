using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "New Health Effect", menuName = "Abilities/Effects/New Health Effect")]
    public class HealthEffect : EffectStrategy
    {
        [SerializeField] float healthChange;

        public override void StartEffect(AbilityData abilityData, Action finished)
        {
            foreach(GameObject target in abilityData.GetTargets())
            {
                Health health = target.GetComponent<Health>();
                if (health)
                {
                    if (healthChange < 0) health.TakeDamage(abilityData.GetUser(), -healthChange);
                    else health.Heal(healthChange);
                }
            }

            finished();
        }
    }
}
