using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        //Components
        [SerializeField] TextMeshProUGUI xpText;

        //Variables
        BaseStats baseStats;


        private void Awake()
        {
            baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
        }
        private void Update()
        {
            xpText.text = "Level " + baseStats.GetLevel().ToString();
        }
    }
}
