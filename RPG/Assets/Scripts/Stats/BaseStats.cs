using UnityEngine;
using System;
using GameDevTV.Utils;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        //Components
        Experience experience;

        //Variables
        [Range(1, 10)] [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progressionSheet = null;
        [SerializeField] GameObject levelUpEffectPrefab = null;
        [SerializeField] bool shouldUseModifiers = false;

        private LazyValue<int> currentLevel;

        public event Action OnLevelUp; 

        //Methods
        private void Awake()
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }
        private void OnEnable()
        {
            if (experience != null) experience.OnExperienceGained += UpdateLevel;
        }
        private void OnDisable()
        {
            if (experience != null) experience.OnExperienceGained -= UpdateLevel;
        }
        private void Start()
        {
            currentLevel.ForceInit();
        }
        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                LevelUpEffect();
                if (OnLevelUp != null) OnLevelUp();
            }
        }
        private void LevelUpEffect()
        {
            Instantiate(levelUpEffectPrefab, transform);
        }
        private float GetBaseStat(Stat stat)
        {
            return progressionSheet.GetStat(stat, characterClass, GetLevel());
        }
        private float GetAdditiveModifiers(Stat stat)
        {
            float total = 0;
            if (!shouldUseModifiers) return total;

            foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach(float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }
        private float GetPercentageModifiers(Stat stat)
        {
            float total = 0;
            if (!shouldUseModifiers) return total;

            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }
        private int CalculateLevel()
        {
            if (!experience) return startingLevel;

            float currentXP = experience.GetExperiencePoints();
            int penultimateLevel = progressionSheet.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int i = 1; i < penultimateLevel; i++)
            {
                float XPToLevelUp = progressionSheet.GetStat(Stat.ExperienceToLevelUp, characterClass, i);
                if (currentXP < XPToLevelUp) return i;
            }

            return penultimateLevel + 1;
        }

        //Getters
        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifiers(stat)) * (1 + GetPercentageModifiers(stat)/100);
        }
        public int GetLevel()
        {
            return currentLevel.value;
        }
    }
}
