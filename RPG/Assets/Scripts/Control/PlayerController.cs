using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using UnityEngine.EventSystems;
using System;
using UnityEngine.AI;
using GameDevTV.Inventories;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType cursorType;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        //Components
        private Mover mover;
        private Fighter fighter;
        private Health health;
        private ActionStore actionStore;

        //Variables
        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMeshProjectionDistance = 1f;
        [SerializeField] float sphereCastRadius = 1f;
        [SerializeField] int numberOfAbilities = 6;

        private RaycastHit hit;
        private bool isDraggingUI = false;

        void Awake()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            actionStore = GetComponent<ActionStore>();
            health.OnDeath += DisableFighter;
            GetComponent<Respawner>().OnRespawn += ReenableComponents;
        }
        void Update()
        {
            if (InteractWithUI()) return;
            if (health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }

            UseAbilities();

            if (InteractWithComponent()) return;
            if (HandleMovement()) return;

            SetCursor(CursorType.None);
        }

        public static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        private bool HandleMovement()
        {
            if (!mover) return false;

            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit)
            {

                if (!mover.CanMoveTo(target)) return false;

                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(target, 1f);
                }

                SetCursor(CursorType.Movement);
                return true;
            }

            return false;
        }
        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();

            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;

            return true;
        }
        private bool InteractWithUI()
        {
            if (Input.GetMouseButtonUp(0)) isDraggingUI = false;
            bool isOverUI = EventSystem.current.IsPointerOverGameObject();
            if (isOverUI)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isDraggingUI = true;
                }
                SetCursor(CursorType.OverUI);
            }
            if (isDraggingUI) return true;
            return isOverUI;
        }
        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach(RaycastHit hit  in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach(IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }
        private RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), sphereCastRadius);
            float[] distances = new float[hits.Length];

            for(int i = 0; i < distances.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            Array.Sort(distances, hits);
            return hits;
        }
        private void SetCursor(CursorType cursorType)
        {
            CursorMapping cursorMapping = GetCursorMapping(cursorType);
            Cursor.SetCursor(cursorMapping.texture, cursorMapping.hotspot, CursorMode.Auto);
        }
        private CursorMapping GetCursorMapping(CursorType cursorType)
        {
            foreach(CursorMapping mapping in cursorMappings)
            {
                if (mapping.cursorType == cursorType) return mapping;
            }

            return cursorMappings[0];
        }
        private void DisableFighter()
        {
            if (!fighter) return;
            fighter.enabled = false;
        }

        private void UseAbilities()
        {
            for(int i = 0; i < numberOfAbilities; i++)
            {
                if(Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    actionStore.Use(i, this.gameObject);
                }
            }
        }

        private void ReenableComponents()
        {
            fighter.enabled = true;
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
