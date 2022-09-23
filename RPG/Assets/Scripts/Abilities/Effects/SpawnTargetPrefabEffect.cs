using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "New Spawn Prefab Effect", menuName = "Abilities/Effects/New Spawn Prefab Effect")]
    public class SpawnTargetPrefabEffect : EffectStrategy
    {
        [SerializeField] Transform prefabToSpawn;
        [SerializeField] float destroyDelayTime = -1;

        public override void StartEffect(AbilityData abilityData, Action finished)
        {
            abilityData.StartCoroutine(Effect(abilityData, finished));
        }

        private IEnumerator Effect(AbilityData abilityData, Action finished)
        {
            Transform instance = Instantiate(prefabToSpawn);
            instance.position = abilityData.GetTargetedPoint();
            
            if(destroyDelayTime > 0)
            {
                yield return new WaitForSeconds(destroyDelayTime);
                Destroy(instance.gameObject);
            }

            finished();
        }
    }
}

