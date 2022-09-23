using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using System;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Delayed Click Targeting", menuName = "Abilities/Targeting/Delaying Click Targeting", order = 0)]
    public class DelayedClickTargeting : TargetingStrategy
    {
        [SerializeField] Texture2D cursorTexture;
        [SerializeField] Vector2 cursorHotSpot;
        [SerializeField] LayerMask layerMask;
        [SerializeField] float areaAffectRadius = 10;
        [SerializeField] Transform targetingPrefab;

        private Transform targetingPrefabInstance;

        public override void StartTargeting(AbilityData abilityData, Action finished)
        {
            PlayerController playerController = abilityData.GetUser().GetComponent<PlayerController>();
            playerController.StartCoroutine(Targeting(abilityData, playerController, finished));
        }

        private IEnumerator Targeting(AbilityData abilityData, PlayerController playerController, Action finished)
        {
            if (targetingPrefabInstance == null) targetingPrefabInstance = Instantiate(targetingPrefab);
            else targetingPrefabInstance.gameObject.SetActive(true);

            targetingPrefabInstance.localScale = new Vector3(areaAffectRadius * 2, 1, areaAffectRadius * 2);

            playerController.enabled = false;
            while (!abilityData.IsCancelled())
            {
                Cursor.SetCursor(cursorTexture, cursorHotSpot, CursorMode.Auto);
                Cursor.visible = true;

                RaycastHit raycastHit;
                if (Physics.Raycast(PlayerController.GetMouseRay(), out raycastHit, 1000, layerMask))
                {
                    targetingPrefabInstance.position = raycastHit.point;

                    if (Input.GetMouseButtonDown(0))
                    {
                        yield return new WaitWhile(() => Input.GetMouseButton(0));
                        abilityData.SetTargetedPoint(raycastHit.point);
                        abilityData.SetTargets(GetGameObjectsInRadius(raycastHit.point));
                        break;
                    }
                }

               yield return null;
            }

            targetingPrefabInstance.gameObject.SetActive(false);
            playerController.enabled = true;
            finished();
        }

        private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
        {
            RaycastHit[] hits = Physics.SphereCastAll(point, areaAffectRadius, Vector3.up, 0);
            foreach(var hit in hits)
            {
                yield return hit.collider.gameObject;
            }
                      
        }
    }
}
