using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Projectiles;
using RPG.Attributes;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "New Spawn Projectile Effect", menuName = "Abilities/Effects/New Spawn Projectile Effect")]
    public class SpawnProjectileEffect : EffectStrategy
    {
        [SerializeField] Projectile projectileToSpawn;
        [SerializeField] float damage;
        [SerializeField] bool isRightHand = true;

        public override void StartEffect(AbilityData abilityData, Action finished)
        {
            Fighter fighter = abilityData.GetUser().GetComponent<Fighter>();
            GameObject instigator = abilityData.GetUser();
            Vector3 spawnPosition = fighter.GetHandTransform(isRightHand).position;

            foreach (var target in abilityData.GetTargets())
            {
                Health health = target.GetComponent<Health>();
                if (health == null) continue;
                Projectile projectile = Instantiate(projectileToSpawn);
                projectile.transform.position = spawnPosition;
                projectile.SetTarget(instigator, health, damage);
            }

            finished();
        }
    }
}

