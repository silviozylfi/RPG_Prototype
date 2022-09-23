using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class UISwitcher : MonoBehaviour
    {
        [SerializeField] GameObject entryPoint;

        private void Start()
        {
            SwitchTo(entryPoint);
        }

        public void SwitchTo(GameObject ToDisplay)
        {
            if (ToDisplay.transform.parent != transform) return;

            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(child.gameObject == ToDisplay);
            }
        }
    }
}
