using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameDevTV.Saving;
using GameDevTV.Inventories;

namespace RPG.Inventories
{
    public class Purse : MonoBehaviour, ISaveable, IItemStore
    {
        [SerializeField] float startingBalance = 400f;
        float balance = 0;

        public event Action onChange;

        private void Awake()
        {
            balance = startingBalance;
        }

        private void Start()
        {
            if (onChange != null) onChange();
        }

        public float GetBalance()
        {
            return balance;
        }
        public void UpdateBalance(float amount)
        {
            balance += amount;
            if (onChange != null) onChange();
        }

        public object CaptureState()
        {
            return balance;
        }

        public void RestoreState(object state)
        {
            balance = (float)state;
        }

        public int AddItems(InventoryItem item, int number)
        {
            if(item is CurrencyItem)
            {
                UpdateBalance(item.GetPrice() * number);
                return number;
            }

            return 0;
        }
    }
}
