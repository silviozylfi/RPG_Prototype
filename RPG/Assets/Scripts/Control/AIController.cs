using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using GameDevTV.Utils;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        //Components
        private Fighter fighter;
        private Health health;
        private Mover mover;
        private ActionScheduler actionScheduler;

        //Variables
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] float aggroCooldownTime = 10f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1;
        [SerializeField] float waypointDwellTime = 3;
        [SerializeField] float shoutDistance = 5f;
        [SerializeField] bool randomMovement = true;
        GameObject player;
        LazyValue<Vector3> guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;
        int currentWaypointIndex = 0;
        float patrolSpeedFraction;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
            actionScheduler = GetComponent<ActionScheduler>();
            if(health != null && fighter != null) health.OnDeath += DisableFighter;
            patrolSpeedFraction = Random.Range(0.2f, 0.5f);
            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
            guardPosition.ForceInit();
        }

        private void Update()
        {
            if (health != null && health.IsDead()) return;

            if (IsAggrevated())
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        public void Aggrevate()
        {
            timeSinceAggrevated = 0f;
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }
        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }
        private bool IsAggrevated()
        {
            float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceFromPlayer <= chaseDistance || timeSinceAggrevated < aggroCooldownTime;
        }
        private void DisableFighter()
        {
            if (!fighter) return;
            fighter.enabled = false;
        }
        private void AttackBehaviour()
        {
            if (!fighter || !fighter.enabled) return;
            if (fighter.CanAttack(player) || IsAggrevated())
            {
                fighter.Attack(player);
                timeSinceLastSawPlayer = 0;

                AggrevateNearbyEnemies();
            }
            
        }
        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach(RaycastHit hit in hits)
            {
                AIController aiController = hit.transform.GetComponent<AIController>();
                if (!aiController) continue;
                else aiController.Aggrevate();
            }
        }
        private void SuspicionBehaviour()
        {
            actionScheduler.CancelCurrentAction();
        }
        private void PatrolBehaviour()
        {
            Vector3 nextPosition;

            if (!patrolPath) nextPosition = guardPosition.value;
            else
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    waypointDwellTime = Random.Range(0.5f, 5f);
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if(timeSinceArrivedAtWaypoint > waypointDwellTime) mover.StartMoveAction(nextPosition, patrolSpeedFraction);
        }
        private bool AtWaypoint()
        {
            return Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointTolerance;
        }
        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }
        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex, randomMovement);
        }

        public void Reset()
        {
            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
            if (navMeshAgent != null)
            {
                navMeshAgent.Warp(guardPosition.value);
            }

            timeSinceLastSawPlayer = Mathf.Infinity;
            timeSinceArrivedAtWaypoint = Mathf.Infinity;
            timeSinceAggrevated = Mathf.Infinity;
            currentWaypointIndex = 0;
        }

        //Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
