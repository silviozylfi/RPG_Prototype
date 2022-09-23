using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG.Attributes
{
    public class ManaDisplay : MonoBehaviour
    {
        Mana mana;
        TraitStore traitStore;
        [SerializeField] TextMeshProUGUI manaText;

        private void Awake()
        {
            mana = GameObject.FindGameObjectWithTag("Player").GetComponent<Mana>();
            mana.OnManaChanged += UpdateDisplay;

            traitStore = GameObject.FindGameObjectWithTag("Player").GetComponent<TraitStore>();
            traitStore.OnTraitStoreChange += UpdateDisplay;
        }

        private void Start()
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            manaText.text = (int)mana.GetMana() + "/" + mana.GetMaxMana();
        }
    }
}
