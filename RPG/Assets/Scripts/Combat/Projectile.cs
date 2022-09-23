using UnityEngine;
using RPG.Attributes;

namespace RPG.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        //Variables
        [SerializeField] float speed = 1f;
        [SerializeField] bool followsTarget = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float destroyingDelay = 0;

        GameObject instigator = null;
        Health target = null;
        float damage = 0f;
        bool moving = true;

        private void Start()
        {
            if (!target) return;
            transform.LookAt(GetAimLocation());
        }
        private void Update()
        {
            if (followsTarget && !target.IsDead()) transform.LookAt(GetAimLocation());
            if(moving) transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(GameObject instigator, Health target, float damage)
        {
            this.instigator = instigator;
            this.target = target;
            this.damage = damage;
        }
        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (!targetCapsule) return target.transform.position;
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;
            target.TakeDamage(instigator, damage);
            if (hitEffect != null) Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            moving = false;
            Destroy(gameObject, destroyingDelay);
        }
    }
}
