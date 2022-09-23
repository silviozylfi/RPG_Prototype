using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RPG.Quests;

namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Transform objectivesContainer;
        [SerializeField] GameObject objectivePrefab;
        [SerializeField] GameObject objectiveIncompletePrefab;
        [SerializeField] TextMeshProUGUI rewardText;

        public void Setup(QuestStatus questStatus)
        {
            Quest quest = questStatus.GetQuest();
            title.text = quest.name;

            foreach(Transform child in objectivesContainer)
            {
                Destroy(child.gameObject);
            }

            foreach(var objective in quest.GetObjectives())
            {
                GameObject prefab = objectiveIncompletePrefab;
                if (questStatus.IsObjectiveComplete(objective.reference))
                {
                    prefab = objectivePrefab;
                }
                GameObject objectiveInstance = Instantiate(prefab, objectivesContainer);
                objectiveInstance.GetComponentInChildren<TextMeshProUGUI>().text = objective.description;
            }

            rewardText.text = GetRewardText(quest);
        }

        private string GetRewardText(Quest quest)
        {
            string rewardText = "";
            foreach(var reward in quest.GetRewards())
            {
                if(rewardText != "")
                {
                    rewardText += ",";
                }
                if (reward.number > 1)
                {
                    rewardText += reward.number + " ";
                }
                rewardText += reward.item.GetDisplayName();
            }
            if (rewardText == "")
            {
                rewardText += "No rewards";
            }
            rewardText += ".";
            return rewardText;
        }
    }
}
