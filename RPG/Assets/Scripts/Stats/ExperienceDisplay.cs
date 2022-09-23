using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        //Components
        Text xpText;

        //Variables
        Experience experience;


        private void Awake()
        {
            experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
            xpText = GetComponent<Text>();
        }
        private void Update()
        {
            xpText.text = experience.GetExperiencePoints().ToString("F0");
        }
    }
}
