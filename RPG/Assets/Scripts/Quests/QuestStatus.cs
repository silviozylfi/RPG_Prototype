using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestStatus
    {
        Quest quest;
        List<string> completedObjectives = new List<string>();

        [System.Serializable]
        class QuestStatusRecord
        {
            public string questName;
            public List<string> completedObjectives = new List<string>();
        }

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
        }
        public QuestStatus(object objectState)
        {
            QuestStatusRecord state = objectState as QuestStatusRecord;
            if (state != null)
            {
                quest = Quest.GetByName(state.questName);
                completedObjectives = state.completedObjectives;
            }
        }
        public Quest GetQuest()
        {
            return quest;
        }
        public int GetCompletedCount()
        {
            return completedObjectives.Count;
        }
        public bool IsObjectiveComplete(string objective)
        {
            return completedObjectives.Contains(objective);
        }
        public void CompleteObjective(string objective)
        {
            if (quest.HasObjective(objective))
            {
                completedObjectives.Add(objective);
            }
        }

        public bool IsComplete()
        {
            foreach(var objective in quest.GetObjectives())
            {
                if (!completedObjectives.Contains(objective.reference))
                {
                    return false;
                }
            }

            return true;
        }

        public object CaptureState()
        {
            QuestStatusRecord state = new QuestStatusRecord();
            state.questName = quest.name;
            state.completedObjectives = completedObjectives;
            return state;
        }
    }
}
