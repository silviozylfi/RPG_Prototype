using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Shops
{
    public class Shopper : MonoBehaviour
    {
        Shop activeShop = null;
        public event Action onActiveShopChanged;
        public void SetActiveShop(Shop newShop)
        {
            if (activeShop != null)
            {
                activeShop.SetShopper(null);
            }

            activeShop = newShop;

            if (activeShop != null)
            {
                activeShop.SetShopper(this);
            }

            if (onActiveShopChanged != null) onActiveShopChanged();
        }
        public Shop GetActiveShop()
        {
            return activeShop;
        }
    }
}
