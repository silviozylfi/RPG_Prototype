using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RPG.Inventories;

namespace RPG.UI
{
    public class PurseUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI moneyField;
        Purse purse = null;

        private void Awake()
        {
            purse = GameObject.FindGameObjectWithTag("Player").GetComponent<Purse>();
            purse.onChange += RefreshUI;
        }

        private void RefreshUI()
        {
            moneyField.text = "$" + purse.GetBalance().ToString("0.00");
        }
    }
}
