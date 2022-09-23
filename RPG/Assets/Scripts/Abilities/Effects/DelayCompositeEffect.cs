using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "New Delay Composite Effect", menuName = "Abilities/Effects/New Delay Composite Effect")]
    public class DelayCompositeEffect : EffectStrategy
    {
        [SerializeField] float delay = 0;
        [SerializeField] EffectStrategy[] delayedEffects;
        [SerializeField] bool abortIfCanceled = false;

        public override void StartEffect(AbilityData abilityData, Action finished)
        {
            abilityData.StartCoroutine(DelayedEffect(abilityData, finished));
        }

        private IEnumerator DelayedEffect(AbilityData data, Action finished)
        {
            yield return new WaitForSeconds(delay);

            if (data.IsCancelled() && abortIfCanceled) yield break;

            foreach(EffectStrategy effect in delayedEffects)
            {
                effect.StartEffect(data, finished);
            }
        }
    }
}
