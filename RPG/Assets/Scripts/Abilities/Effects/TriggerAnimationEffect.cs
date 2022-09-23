using System;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "New Trigger Animation Effect", menuName = "Abilities/Effects/New Trigger Animation Effect")]
    public class TriggerAnimationEffect : EffectStrategy
    {
        [SerializeField] string parameter;

        public override void StartEffect(AbilityData abilityData, Action finished)
        {
            Animator anim = abilityData.GetUser().GetComponent<Animator>();
            anim.SetTrigger(parameter);
            finished();
        }
    }
}
