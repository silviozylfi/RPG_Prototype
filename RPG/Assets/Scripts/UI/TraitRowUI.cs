using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RPG.Stats;

namespace RPG.UI
{
    public class TraitRowUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI traitValue;
        [SerializeField] Button minusButton;
        [SerializeField] Button plusButton;
        [SerializeField] Trait trait;

        TraitStore traitStore;
        BaseStats baseStats;

        private void Awake()
        {
            traitStore = GameObject.FindGameObjectWithTag("Player").GetComponent<TraitStore>();
            traitStore.OnTraitStoreChange += RefreshUI;
            baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
            baseStats.OnLevelUp += RefreshUI;
        }

        private void Start()
        {
            RefreshUI();
        }

        public void AllocatePoints(int points)
        {
            traitStore.AssignPoints(trait, points);
        }

        private void RefreshUI()
        {
            traitValue.text = traitStore.GetProposedPoints(trait).ToString();
            minusButton.interactable = traitStore.CanAssignPoints(trait, -1);
            plusButton.interactable = traitStore.CanAssignPoints(trait, 1);
        }
    }
}
