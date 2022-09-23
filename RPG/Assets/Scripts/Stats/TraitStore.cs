using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameDevTV.Saving;

namespace RPG.Stats
{
    public class TraitStore : MonoBehaviour, IModifierProvider, ISaveable
    {
        [SerializeField] TraitBonus[] bonusConfig;
        [System.Serializable]
        class TraitBonus
        {
            public Trait trait;
            public Stat stat;
            public float additiveBonusPerPoint;
            public float percentageBonusPerPoint;
        }

        Dictionary<Trait, int> assignedPoints = new Dictionary<Trait, int>();
        Dictionary<Trait, int> stagedPoints = new Dictionary<Trait, int>();

        Dictionary<Stat, Dictionary<Trait, float>> additiveBonusCache;
        Dictionary<Stat, Dictionary<Trait, float>> percentageBonusCache;

        public event Action OnTraitStoreChange;

        private void Awake()
        {
            additiveBonusCache = new Dictionary<Stat, Dictionary<Trait, float>>();
            percentageBonusCache = new Dictionary<Stat, Dictionary<Trait, float>>();

            foreach (TraitBonus traitBonus in bonusConfig)
            {
                if (!additiveBonusCache.ContainsKey(traitBonus.stat))
                {
                    additiveBonusCache[traitBonus.stat] = new Dictionary<Trait, float>();
                }

                if (!percentageBonusCache.ContainsKey(traitBonus.stat))
                {
                    percentageBonusCache[traitBonus.stat] = new Dictionary<Trait, float>();
                }

                additiveBonusCache[traitBonus.stat][traitBonus.trait] = traitBonus.additiveBonusPerPoint;
                percentageBonusCache[traitBonus.stat][traitBonus.trait] = traitBonus.percentageBonusPerPoint;
            }
        }

        public int GetPoints(Trait trait)
        {
            return assignedPoints.ContainsKey(trait) ? assignedPoints[trait] : 0;
        }

        public void AssignPoints(Trait trait, int points)
        {
            if (!CanAssignPoints(trait, points)) return;
            stagedPoints[trait] = GetStagedPoints(trait) + points;

            OnTraitStoreChange?.Invoke();
        }

        public int GetUnassignedPoints()
        {
            return GetAssignablePoints() - GetTotalProposedPoints();
        }

        public int GetStagedPoints(Trait trait)
        {
            return stagedPoints.ContainsKey(trait) ? stagedPoints[trait] : 0;
        }

        public int GetProposedPoints(Trait trait)
        {
            return GetStagedPoints(trait) + GetPoints(trait);
        }

        public bool CanAssignPoints(Trait trait, int points)
        {
            if (GetStagedPoints(trait) + points < 0) return false;
            if (GetUnassignedPoints() < points) return false;
            return true;
        }

        public void Commit()
        {
            foreach(Trait trait in stagedPoints.Keys)
            {
                assignedPoints[trait] = GetProposedPoints(trait);
            }

            stagedPoints.Clear();
            OnTraitStoreChange?.Invoke();
        }

        public int GetAssignablePoints()
        {
            return (int)GetComponent<BaseStats>().GetStat(Stat.TotalTraitPoints);
        }

        private int GetTotalProposedPoints()
        {
            int total = 0;

            foreach(int points in assignedPoints.Values)
            {
                total += points;
            }

            foreach (int points in stagedPoints.Values)
            {
                total += points;
            }

            return total;
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (!additiveBonusCache.ContainsKey(stat))
            {
                yield break;
            }
            foreach(Trait trait in additiveBonusCache[stat].Keys)
            {
                float bonus = additiveBonusCache[stat][trait];
                yield return bonus * GetPoints(trait);
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (!percentageBonusCache.ContainsKey(stat))
            {
                yield break;
            }
            foreach (Trait trait in percentageBonusCache[stat].Keys)
            {
                float bonus = percentageBonusCache[stat][trait];
                yield return bonus * GetPoints(trait);
            }
        }

        public object CaptureState()
        {
            return assignedPoints;
        }

        public void RestoreState(object state)
        {
            assignedPoints = new Dictionary<Trait, int>((Dictionary<Trait, int>)state);
        }
    }
}
