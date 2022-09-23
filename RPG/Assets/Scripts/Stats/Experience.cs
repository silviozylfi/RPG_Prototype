using GameDevTV.Saving;
using UnityEngine;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        //Variables
        [SerializeField] float experiencePoints = 0;

        public event Action OnExperienceGained;

        //Methods
        public void GainExperience(float amount)
        {
            experiencePoints += amount;
            if (OnExperienceGained != null) OnExperienceGained();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.E))
            {
                GainExperience(Time.deltaTime * 20);
            }
        }

        //Getters
        public float GetExperiencePoints()
        {
            return experiencePoints;
        }

        //Saving System
        public object CaptureState()
        {
            return experiencePoints;
        }
        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}
