using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;
using RPG.Core;

namespace RPG.Abilities
{
    [CreateAssetMenu(fileName = "New Ability", menuName = "Abilities/New Ability")]
    public class Ability : ActionItem
    {
        [SerializeField] TargetingStrategy targetingStrategy;
        [SerializeField] FilterStrategy[] filterStrategies;
        [SerializeField] EffectStrategy[] effectStrategies;
        [SerializeField] float cooldownTime = 0;
        [SerializeField] float manaCost = 20;

        public override void Use(GameObject user)
        {
            Mana mana = user.GetComponent<Mana>();
            if (mana.GetMana() < manaCost) return;

            CooldownStore cooldownStore = user.GetComponent<CooldownStore>();
            if (cooldownStore.GetTimeRemaining(this) > 0) return; 

            AbilityData abilityData = new AbilityData(user);

            ActionScheduler actionScheduler = user.GetComponent<ActionScheduler>();
            actionScheduler.StartAction(abilityData);

            targetingStrategy.StartTargeting(abilityData, () =>
            {
                TargetAcquired(abilityData);
            });
        }

        private void TargetAcquired(AbilityData abilityData)
        {
            if (abilityData.IsCancelled()) return;

            Mana mana = abilityData.GetUser().GetComponent<Mana>();
            if(!mana.UseMana(manaCost)) return;

            CooldownStore cooldownStore = abilityData.GetUser().GetComponent<CooldownStore>();
            cooldownStore.StartCooldown(this, cooldownTime);

            foreach (FilterStrategy filterStrategy in filterStrategies)
            {
                abilityData.SetTargets(filterStrategy.Filter(abilityData.GetTargets()));
            }

            foreach(EffectStrategy effectStrategy in effectStrategies)
            {
                effectStrategy.StartEffect(abilityData, EffectFinished);
            }
        }

        private void EffectFinished()
        {

        }
    }
}
