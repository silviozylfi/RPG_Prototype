using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using RPG.Shops;

namespace RPG.UI.Shops
{
    public class RowUI : MonoBehaviour
    {
        [SerializeField] Image iconField;
        [SerializeField] TextMeshProUGUI nameField;
        [SerializeField] TextMeshProUGUI availabilityField;
        [SerializeField] TextMeshProUGUI priceField;
        [SerializeField] TextMeshProUGUI quantityField;

        Shop currentShop = null;
        ShopItem item = null;
       
        public void Setup(ShopItem item, Shop shop)
        {
            currentShop = shop;
            this.item = item;

            iconField.sprite = item.GetIcon();
            nameField.text = item.GetName();
            availabilityField.text = item.GetAvailability().ToString();
            priceField.text = "$" + item.GetPrice().ToString(".00");
            quantityField.text = item.GetQuantityInTransaction().ToString();
        }

        public void Add()
        {
            currentShop.AddToTransaction(item.GetInventoryItem(), 1);
        }
        public void Remove()
        {
            currentShop.AddToTransaction(item.GetInventoryItem(), -1);
        }
    }
}
