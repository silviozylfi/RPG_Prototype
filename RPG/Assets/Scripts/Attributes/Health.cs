using UnityEngine;
using System;
using GameDevTV.Saving;
using RPG.Stats;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        //Components
        Animator animator;
        BaseStats baseStats;

        //Variables
        private LazyValue<float> healthPoints;
        [SerializeField] private bool wasDeadLastFrame = false;

        [SerializeField] UnityEvent<float> takeDamage;
        [SerializeField] UnityEvent damageHealthBar;
        public event Action OnDeath;
        public event Action OnHealthUpdate;

        //Methods
        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();
            animator = GetComponent<Animator>();
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }
        private void Start()
        {
            healthPoints.ForceInit();
        }
        private void OnEnable()
        {
            baseStats.OnLevelUp += RegenerateHealth;
        }
        private void OnDisable()
        {
            baseStats.OnLevelUp -= RegenerateHealth;
        }
        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        private void UpdateState()
        {
            if(!wasDeadLastFrame && IsDead())
            {
                GetComponent<Collider>().enabled = false;
                if (OnDeath != null) OnDeath();
                if (animator) animator.SetTrigger("Die");
            }

            if(wasDeadLastFrame && !IsDead())
            {
                GetComponent<Collider>().enabled = true;
                animator.Rebind();
            }

            wasDeadLastFrame = IsDead();

        }
        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience != null)
            {
                experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
            }
        }
        private void RegenerateHealth()
        {
            healthPoints.value = GetComponent<BaseStats>().GetStat(Stat.Health);
            if (OnHealthUpdate != null) OnHealthUpdate();
        }
        public void TakeDamage(GameObject instigator, float damage)
        {
            takeDamage.Invoke(damage);

            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            damageHealthBar.Invoke();

            if (IsDead())
            {
                AwardExperience(instigator);
            }

            UpdateState();
            if (OnHealthUpdate != null) OnHealthUpdate();
        }
        public void Heal(float healthToRestore)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + healthToRestore, GetComponent<BaseStats>().GetStat(Stat.Health));
            UpdateState();
            if (OnHealthUpdate != null) OnHealthUpdate();
        }

        //Getters
        public bool IsDead() { return healthPoints.value <= 0; }
        public float GetPercentage()
        {
            return GetFraction() * 100;
        }
        public float GetFraction()
        {
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        public float GetHealthPoints()
        {
            return healthPoints.value;
        }
        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        //Saving System
        public object CaptureState()
        {
            return healthPoints.value;
        }
        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;
            UpdateState();
        }
    }
}
