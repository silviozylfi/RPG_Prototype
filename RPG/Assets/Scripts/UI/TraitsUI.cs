using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RPG.Stats;
using UnityEngine.UI;

namespace RPG.UI
{
    public class TraitsUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI unassignedPoints;
        [SerializeField] Button commitButton;
        TraitStore traitStore;
        BaseStats baseStats;

        private void Awake()
        {
            traitStore = GameObject.FindGameObjectWithTag("Player").GetComponent<TraitStore>();
            traitStore.OnTraitStoreChange += RefreshUI;
            baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
            baseStats.OnLevelUp += RefreshUI;
            commitButton.onClick.AddListener(traitStore.Commit);
        }

        private void Start()
        {
            RefreshUI();
        }

        private void RefreshUI()
        {
            unassignedPoints.text = traitStore.GetUnassignedPoints().ToString();
        }
    }
}
