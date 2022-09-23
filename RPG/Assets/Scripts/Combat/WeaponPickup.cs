using UnityEngine;
using GameDevTV.Saving;
using RPG.Control;
using RPG.Movement;
using RPG.Attributes;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, ISaveable, IRaycastable
    {
        //Variables
        [SerializeField] WeaponConfig weapon;
        [SerializeField] private bool alreadyPicked = false;
        [SerializeField] GameObject pickup;
        [SerializeField] ParticleSystem particleSystem;
        [SerializeField] float healthToRestore = 0f;

        private void Start()
        {
            if (alreadyPicked) Deactivate();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject subject)
        {
            if(weapon) subject.GetComponent<Fighter>().EquipWeapon(weapon);
            if (healthToRestore > 0) subject.GetComponent<Health>().Heal(healthToRestore);
            alreadyPicked = true;
            Deactivate();
        }
        private void Deactivate()
        {
            GetComponent<Collider>().enabled = false;
            pickup.SetActive(false);
            particleSystem.Stop();
        }
        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<Mover>().StartMoveAction(transform.position, 1f);
            }

            return true;
        }
        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }

        //Saving System
        public object CaptureState()
        {
            return alreadyPicked;
        }
        public void RestoreState(object state)
        {
            alreadyPicked = (bool)state;
        }
    }
}
