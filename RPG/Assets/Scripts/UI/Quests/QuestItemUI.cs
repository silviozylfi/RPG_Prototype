using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Quests;
using TMPro;

namespace RPG.UI.Quests
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI progress;
        QuestStatus questStatus;

        public void Setup(QuestStatus questStatus)
        {
            this.questStatus = questStatus;
            title.text = this.questStatus.GetQuest().GetTitle();
            progress.text = questStatus.GetCompletedCount() + "/" + this.questStatus.GetQuest().GetObjectivesCount();
        }

        public QuestStatus GetQuestStatus()
        {
            return questStatus;
        }
    }
}
