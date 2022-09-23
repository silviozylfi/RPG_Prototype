using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities
{
    public class CooldownStore : MonoBehaviour
    {
        Dictionary<InventoryItem, float> cooldownTimers = new Dictionary<InventoryItem, float>();
        Dictionary<InventoryItem, float> initialCooldownTimers = new Dictionary<InventoryItem, float>();

        private void Update()
        {
            var keys = new List<InventoryItem>(cooldownTimers.Keys);

            foreach(Ability ability in keys)
            {
                cooldownTimers[ability] -= Time.deltaTime;
                if(cooldownTimers[ability] < 0)
                {
                    cooldownTimers.Remove(ability);
                    initialCooldownTimers.Remove(ability);
                }
            }
        }

        public void StartCooldown(Ability ability, float cooldownTime)
        {
            cooldownTimers[ability] = cooldownTime;
            initialCooldownTimers[ability] = cooldownTime;
        }

        public float GetTimeRemaining(Ability ability)
        {
            if (!cooldownTimers.ContainsKey(ability)) return 0;
            else return cooldownTimers[ability];
        }

        public float GetFractionRemaining(InventoryItem ability)
        {
            if (ability == null) return 0;
            if (!cooldownTimers.ContainsKey(ability)) return 0;
            else return cooldownTimers[ability] / initialCooldownTimers[ability];
        }
    }
}
