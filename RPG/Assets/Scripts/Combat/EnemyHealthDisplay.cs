using UnityEngine;
using UnityEngine.UI;
using RPG.Combat;

namespace RPG.Attributes
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        //Components
        Text healthText;

        //Variables
        Fighter playerFighter;


        private void Awake()
        {
            playerFighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
            healthText = GetComponent<Text>();
        }
        private void Update()
        {
            Health currentTarget = playerFighter.GetTarget();
            if (currentTarget)
            {
                healthText.text = currentTarget.GetHealthPoints().ToString("F0") + "/" + currentTarget.GetMaxHealthPoints().ToString("F0"); ;
            }
            else
            {
                healthText.text = "N/A";
            }
        }
    }

}