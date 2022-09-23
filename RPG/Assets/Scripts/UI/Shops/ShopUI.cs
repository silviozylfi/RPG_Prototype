using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Shops;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI shopName;
        [SerializeField] Transform listRoot;
        [SerializeField] RowUI rowUIPrefab;
        [SerializeField] TextMeshProUGUI totalField;
        [SerializeField] Button confirmButton;
        [SerializeField] TextMeshProUGUI switchText;

        Shopper playerShopper;
        Shop currentShop = null;
        Color originalTotalTextColor;

        private void Awake()
        {
            playerShopper = GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>();
            if (playerShopper == null) return;
            playerShopper.onActiveShopChanged += ShopChanged;
            originalTotalTextColor = totalField.color;
        }

        private void Start()
        {
            ShopChanged();
        }

        private void ShopChanged()
        {
            if (currentShop != null) currentShop.onChange -= RefreshUI;
            currentShop = playerShopper.GetActiveShop();
            gameObject.SetActive(currentShop != null);

            foreach(FilterButtonUI button in GetComponentsInChildren<FilterButtonUI>())
            {
                button.SetShop(currentShop);
            }

            if (currentShop == null) return;

            currentShop.onChange += RefreshUI;
            RefreshUI();
        }
        public void Close()
        {
            currentShop.onChange -= RefreshUI;
            playerShopper.SetActiveShop(null);
        }

        public void Transact()
        {
            currentShop.ConfirmTransaction();
        }

        public void SwitchMode()
        {
            currentShop.SwitchMode();
        }

        private void RefreshUI()
        {
            shopName.text = currentShop.GetShopName();

            foreach(Transform child in listRoot)
            {
                Destroy(child.gameObject);
            }

            foreach(ShopItem item in currentShop.GetFilteredItems())
            {
                RowUI row = Instantiate<RowUI>(rowUIPrefab, listRoot);
                row.Setup(item, currentShop);
            }

            totalField.text = "Total: $" + currentShop.TransactionTotal().ToString("0.00");
            totalField.color = currentShop.HasSufficientFunds() ? originalTotalTextColor : Color.red;
            confirmButton.interactable = currentShop.CanTransact();
            if (currentShop.IsBuyingMode())
            {
                switchText.text = "Switch To Selling";
                confirmButton.GetComponentInChildren<TextMeshProUGUI>().text = "Buy";
            }
            else
            {
                switchText.text = "Switch To Buying";
                confirmButton.GetComponentInChildren<TextMeshProUGUI>().text = "Sell";
            }

            foreach (FilterButtonUI button in GetComponentsInChildren<FilterButtonUI>())
            {
                button.RefreshUI();
            }
        }
    }
}
