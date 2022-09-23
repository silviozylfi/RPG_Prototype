using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameDevTV.Inventories;
using RPG.Shops;

namespace RPG.UI.Shops
{
    public class FilterButtonUI : MonoBehaviour
    {
        [SerializeField] ItemCategory itemCategory = ItemCategory.None;
        Button button;
        Shop currentShop;
        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(SelectFilter);
        }
        public void SetShop(Shop shop)
        {
            currentShop = shop;
        }
        public void RefreshUI()
        {
            button.interactable = currentShop.GetFilter() != itemCategory;
        }
        private void SelectFilter()
        {
            currentShop.SelectFilter(itemCategory);
        }
    }
}
