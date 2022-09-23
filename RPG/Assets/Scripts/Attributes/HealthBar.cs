using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] RectTransform foreground;
        [SerializeField] Health health;
        [SerializeField] Canvas rootCanvas;

        private void Start()
        {
            CheckHealthBar();
        }

        public void UpdateHealthBar()
        {
            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(health.GetFraction(), 1f, 1f);
            CheckHealthBar();
        }
        private void CheckHealthBar()
        {
            if (Mathf.Approximately(health.GetFraction(), 0)
                || Mathf.Approximately(health.GetFraction(), 1)) rootCanvas.enabled = false;
        }
    }
}
