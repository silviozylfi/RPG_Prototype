using UnityEngine;
using RPG.Attributes;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        //Components
        private Health health;
        //Variables
        IAction currentAction;

        private void Awake()
        {
            health = GetComponent<Health>();
            if (!health) return;
            health.OnDeath += CancelCurrentAction;
        }

        public void StartAction(IAction action)
        {
            if (currentAction == action) return;
            if (currentAction != null) currentAction.Cancel();
            currentAction = action;
        }
        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}
