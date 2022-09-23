using UnityEngine;
using RPG.Movement;
using RPG.Core;
using GameDevTV.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;
using GameDevTV.Inventories;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        //Components
        private Mover mover;
        private ActionScheduler actionScheduler;
        private Animator animator;

        //Variables
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        [SerializeField] string defaultWeaponName = "Unarmed";
        [SerializeField] float autoAttackRange = 5f;

        private Health target;
        private float timeSinceLastAttack = Mathf.Infinity;
        private WeaponConfig currentWeaponConfig;
        private LazyValue<Weapon> currentWeapon;
        private Equipment equipment;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
            if (currentWeaponConfig == null) EquipWeapon(defaultWeapon);
            equipment = GetComponent<Equipment>();
            if (equipment)
            {
                equipment.equipmentUpdated += UpdateWeapon;
            }
        }
        private void Start()
        {
            currentWeapon.ForceInit();
        }
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead())
            {
                target = FindNewTargetInRange();
                if (target == null) return;
            }

            if (mover != null && !IsInRange(target.transform))
            {
                mover.MoveTo(target.transform.position, 1f);
            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (!combatTarget) return false;
            if (!mover.CanMoveTo(combatTarget.transform.position) && !IsInRange(combatTarget.transform)) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest && !targetToTest.IsDead();
        }
        public void Attack(GameObject combatTarget)
        {
            //This will trigger the Hit() event.
            if (!actionScheduler) return;
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();         
        }
        public void Cancel()
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("StopAttack");
            target = null;
            GetComponent<Mover>().Cancel();
        }
        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        private void UpdateWeapon()
        {
            var weapon = equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;
            if (!weapon) EquipWeapon(defaultWeapon);
            else EquipWeapon(weapon);
        }
        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }
        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }
        private bool IsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < currentWeaponConfig.GetWeaponRange();
        }
        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if(timeSinceLastAttack > timeBetweenAttacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0f;
            }
        }
        private void TriggerAttack()
        {
            animator.ResetTrigger("StopAttack");
            animator.SetTrigger("Attack");
        }

        private Health FindNewTargetInRange()
        {
            Health best = null;
            float bestDistance = Mathf.Infinity;

            foreach(Health candidate in FindAllTargetsInRange())
            {
                float candidateDistance = Vector3.Distance(-transform.position, candidate.transform.position);
                if(candidateDistance < bestDistance)
                {
                    bestDistance = candidateDistance;
                    best = candidate;
                }
            }

            return best;
        }

        private IEnumerable<Health> FindAllTargetsInRange()
        {
            RaycastHit[] raycastHits = Physics.SphereCastAll(transform.position, autoAttackRange, Vector3.up);
            foreach (RaycastHit hit in raycastHits)
            {
                Health hitHealth = hit.transform.GetComponent<Health>();
                if (hitHealth == null) continue;
                if (hitHealth.IsDead()) continue;
                if (hitHealth.gameObject == gameObject) continue;
                yield return hitHealth;
            }
        }

        //Animation Event
        private void Hit()
        {
            if (!target) return;

            float calculatedDamage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            BaseStats targetBaseStats = target.GetComponent<BaseStats>();

            if (targetBaseStats != null)
            {
                float defence = targetBaseStats.GetStat(Stat.Defence);
                calculatedDamage /= 1 + defence / calculatedDamage;
            }


            if (currentWeapon.value) currentWeapon.value.OnHit();

            if (currentWeaponConfig.HasProjectile()) currentWeaponConfig.Launch(gameObject, rightHandTransform, leftHandTransform, target, calculatedDamage);
            else target.TakeDamage(gameObject, calculatedDamage);
        }
        private void Shoot()
        {
            Hit();
        }

        //Getters
        public Health GetTarget()
        {
            return target;
        }

        public Transform GetHandTransform(bool isRightHand)
        {
            if (isRightHand) return rightHandTransform;
            else return leftHandTransform;
        }
    }
}
