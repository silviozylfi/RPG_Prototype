using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        //Variables
        [SerializeField] Text damageText = null;

        public void SetValue(float amount)
        {
            damageText.text = amount.ToString("F0");
        }

        //Called by Animation
        public void DestroyAfterFading()
        {
            Destroy(gameObject);
        }
    }
}
