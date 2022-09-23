using UnityEngine;
using UnityEngine.UI;
using System;
using RPG.Stats;
using TMPro;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        //Components
        [SerializeField] TextMeshProUGUI healthText;

        //Variables
        Health health;
        TraitStore traitStore;


        private void Awake()
        {
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            health.OnHealthUpdate += UpdateDisplay;

            traitStore = GameObject.FindGameObjectWithTag("Player").GetComponent<TraitStore>();
            traitStore.OnTraitStoreChange += UpdateDisplay;
        }

        private void Start()
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            healthText.text = health.GetHealthPoints().ToString("F0") + "/" + health.GetMaxHealthPoints().ToString("F0");
        }
    }
}
