using UnityEngine;
using UnityEngine.Playables;
using GameDevTV.Saving;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        //Variables
        [SerializeField] bool alreadyTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if(!alreadyTriggered && other.CompareTag("Player"))
            {
                GetComponent<PlayableDirector>().Play();
                alreadyTriggered = true;
            }

        }

        public object CaptureState()
        {
            return alreadyTriggered;
        }
        public void RestoreState(object state)
        {
            alreadyTriggered = (bool)state;
        }
    }
}
