using UnityEngine;
using GameDevTV.Saving;
using UnityEditor;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        //Components
        SavingSystem savingSystem;

        //Variables
        const string currentSaveKey = "currentSaveName";
        [SerializeField] int mainMenuLevelBuildIndex = 0;
        [SerializeField] int firstLevelBuildIndex = 1;

        private void Awake()
        {
            savingSystem = GetComponent<SavingSystem>();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L)) Load();
            if (Input.GetKeyDown(KeyCode.S)) Save();
            if (Input.GetKeyDown(KeyCode.D)) Delete();
        }
        

        public void Load()
        {
            savingSystem.Load(GetCurrentSave()); 
        }
        public void Save()
        {
            savingSystem.Save(GetCurrentSave());
        }
        public void Delete()
        {
            savingSystem.Delete(GetCurrentSave());
        }

        public void ContinueGame()
        {
            if (!PlayerPrefs.HasKey(currentSaveKey)) return;
            if (!savingSystem.SaveFileExists(GetCurrentSave())) return;
            StartCoroutine(LoadLastScene());
        }

        public void StartNewGame(string saveFile)
        {
            if (String.IsNullOrEmpty(saveFile)) return;
            SetCurrentSave(saveFile);
            StartCoroutine(LoadFirstScene());
        }

        private void SetCurrentSave(string saveFile)
        {
            PlayerPrefs.SetString(currentSaveKey, saveFile);
        }

        private string GetCurrentSave()
        {
            return PlayerPrefs.GetString(currentSaveKey);
        }

        private IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(3f);
            yield return savingSystem.LoadLastScene(GetCurrentSave());
            yield return new WaitForSeconds(1f);
            yield return fader.FadeIn(3f);
        }

        private IEnumerator LoadFirstScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(3f);
            yield return SceneManager.LoadSceneAsync(firstLevelBuildIndex);
            yield return new WaitForSeconds(1f);
            yield return fader.FadeIn(3f);
        }

        private IEnumerator LoadMainMenuScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(3f);
            yield return SceneManager.LoadSceneAsync(mainMenuLevelBuildIndex);
            yield return new WaitForSeconds(1f);
            yield return fader.FadeIn(3f);
        }

        public IEnumerable<string> ListSaves()
        {
            return savingSystem.ListSaves();
        }

        public void LoadGame(string saveFile)
        {
            SetCurrentSave(saveFile);
            ContinueGame();
        }

        public void LoadMainMenu()
        {
            StartCoroutine(LoadMainMenuScene());
        }
    }
}
