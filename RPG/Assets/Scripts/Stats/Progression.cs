using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression Sheet", menuName = "Progression Sheet", order = 0)]
    public class Progression : ScriptableObject 
    {
        //Variables
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;
        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        //Methods
        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();
            if (!(lookupTable[characterClass].ContainsKey(stat))) return 0;

            float[] levels = lookupTable[characterClass][stat];
            if (levels.Length == 0) return 0;
            if (levels.Length < level) return levels[levels.Length - 1];
            return levels[level - 1];
        }
        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookup();
            if (!(lookupTable[characterClass].ContainsKey(stat))) return 0;
            return lookupTable[characterClass][stat].Length;
        }
        private void BuildLookup()
        {
            if (lookupTable == null)
            {
                lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

                foreach(ProgressionCharacterClass pcc in characterClasses)
                {
                    Dictionary<Stat, float[]> statLookupTable = new Dictionary<Stat, float[]>();

                    foreach(ProgressionStat ps in pcc.GetStats())
                    {
                        statLookupTable[ps.GetStat()] = ps.GetValues();
                    }

                    lookupTable[pcc.GetClass()] = statLookupTable;
                }
            }
        }


        [System.Serializable]
        class ProgressionCharacterClass
        {
            [SerializeField] CharacterClass characterClass;
            [SerializeField] ProgressionStat[] stats;

            public CharacterClass GetClass()
            {
                return characterClass;
            }
            public ProgressionStat[] GetStats()
            {
                return stats;
            }
        }

        [System.Serializable]
        class ProgressionStat
        {
            [SerializeField] Stat stat;
            [SerializeField] float[] values;

            //Getters
            public Stat GetStat()
            {
                return stat;
            }
            public float[] GetValues()
            {
                return values;
            }
        }
    }
}
