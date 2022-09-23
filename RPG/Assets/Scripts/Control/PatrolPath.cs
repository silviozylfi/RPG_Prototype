using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        //Variables
        [SerializeField] float gizmoRadius = 0.2f;

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
        public int GetNextIndex(int i, bool random)
        {
            if (random)
            {
                while (true)
                {
                    int nextIndex = Random.Range(0, transform.childCount);
                    if (nextIndex != i) return nextIndex;
                }
            }

            if (i + 1 == transform.childCount) return 0;
            return i + 1;
        }

        //Called by Unity
        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i, false);

                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(GetWaypoint(i), gizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }
    }
}
