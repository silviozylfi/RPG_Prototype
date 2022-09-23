using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Inventories;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName ="Quest", menuName = "RPG/Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [System.Serializable]
        public class Reward
        {
            [Min(1)] public int number;
            public InventoryItem item;
        }
        [System.Serializable]
        public class Objective
        {
            public string reference;
            public string description;
        }

        [SerializeField] List<Objective> objectives = new List<Objective>();
        [SerializeField] List<Reward> rewards = new List<Reward>();

        public string GetTitle()
        {
            return name;
        }
        public int GetObjectivesCount()
        {
            return objectives.Count;
        }
        public IEnumerable<Objective> GetObjectives()
        {
            return objectives;
        }

        public IEnumerable<Reward> GetRewards()
        {
            return rewards;
        }

        public bool HasObjective(string objectiveReference)
        {
            foreach(var objective in objectives)
            {
                if(objective.reference == objectiveReference)
                {
                    return true;
                }
            }

            return false;
        }

        public static Quest GetByName(string questName)
        {
            foreach(Quest quest in Resources.LoadAll<Quest>(""))
            {
                if(quest.name == questName)
                {
                    return quest;
                }
            }

            return null;
        } 
    }
}
