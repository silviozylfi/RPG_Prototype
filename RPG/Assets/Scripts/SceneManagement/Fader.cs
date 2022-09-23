using System.Collections;
using UnityEngine;
using RPG.Core;
using RPG.Control;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        //Components
        CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            StartCoroutine(FadeIn(4f));
        }
        public IEnumerator FadeOut(float time)
        {
            canvasGroup.alpha = 0;
            DisableControl();

            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.unscaledDeltaTime / time;
                yield return null;
            }
        }
        public IEnumerator FadeIn(float time)
        {
            canvasGroup.alpha = 1;
            DisableControl();

            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.unscaledDeltaTime / time;
                yield return null;
            }

            EnableControl();
        }

        void DisableControl()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }
        void EnableControl()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}
