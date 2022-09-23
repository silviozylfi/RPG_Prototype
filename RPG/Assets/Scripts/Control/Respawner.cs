using Cinemachine;
using RPG.Attributes;
using RPG.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class Respawner : MonoBehaviour
    {
        [SerializeField] Transform respawnLocation;
        [SerializeField] float respawnDelay = 3f;
        [SerializeField] float fadeTime = 3f;
        [SerializeField] float healthRegenPercentage = 50;
        [SerializeField] float enemyHealthRecenPercentage = 20;

        public event Action OnRespawn;

        private void Awake()
        {
            GetComponent<Health>().OnDeath += Respawn;
        }

        private void Start()
        {
            if (GetComponent<Health>().IsDead())
            {
                Respawn();
            }
        }

        private void Respawn()
        {
            StartCoroutine(RespawnRoutine());
        }

        private IEnumerator RespawnRoutine()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
            yield return new WaitForSeconds(respawnDelay);
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeTime);
            ResetPlayer();
            ResetEnemies();
            savingWrapper.Save();
            yield return new WaitForSeconds(2f);
            OnRespawn?.Invoke();
            yield return fader.FadeIn(fadeTime);
        }

        private void ResetPlayer()
        {
            Vector3 positionDelta = respawnLocation.position - transform.position;
            GetComponent<NavMeshAgent>().Warp(respawnLocation.position);
            Health health = GetComponent<Health>();
            health.Heal(health.GetMaxHealthPoints() * healthRegenPercentage / 100);
            ICinemachineCamera activeCamera = FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
            if(activeCamera.Follow == this.transform)
            {
                activeCamera.OnTargetObjectWarped(transform, positionDelta);
            }
        }

        private void ResetEnemies()
        {
            foreach(AIController enemyController in FindObjectsOfType<AIController>())
            {              
                Health enemyHealth = enemyController.gameObject.GetComponent<Health>();
                if (enemyHealth != null && !enemyHealth.IsDead())
                {
                    enemyController.Reset();
                    enemyHealth.Heal(enemyHealth.GetMaxHealthPoints() * enemyHealthRecenPercentage / 100);
                }
            }
        }
    }
}
