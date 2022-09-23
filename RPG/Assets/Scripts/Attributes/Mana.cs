using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameDevTV.Utils;
using RPG.Stats;
using GameDevTV.Saving;

namespace RPG.Attributes
{
    public class Mana : MonoBehaviour, ISaveable
    {
        LazyValue<float> mana;
        BaseStats baseStats;

        public event Action OnManaChanged;


        private void Awake()
        {
            mana = new LazyValue<float>(GetMaxMana);
            baseStats = GetComponent<BaseStats>();
        }

        private void Update()
        {
            if (mana.value < GetMaxMana())
            {
                mana.value += GetRegenRate() * Time.deltaTime;
                if (mana.value > GetMaxMana()) mana.value = GetMaxMana();
                OnManaChanged?.Invoke();
            }
        }

        public float GetMaxMana()
        {
            return baseStats.GetStat(Stat.Mana);
        }

        public float GetRegenRate()
        {
            return baseStats.GetStat(Stat.ManaRegenRate);
        }
        public float GetMana()
        {
            return mana.value;
        }
        public bool UseMana(float manaToUse)
        {
            if (manaToUse > mana.value)
            {
                return false;
            }

            mana.value -= manaToUse;
            OnManaChanged?.Invoke();
            return true;
        }

        public object CaptureState()
        {
            return mana.value;
        }

        public void RestoreState(object state)
        {
            mana.value = (float)state;
        }
    }
}
