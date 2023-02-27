using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameSceneManager
{
    public static GameSceneManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public event EventHandler onLevelLoaded;

    [SerializeField] GameObject loadingUIPanel;
    List<AsyncOperation> sceneLoading = new List<AsyncOperation>();

    public void LoadGameLevel(LevelInfoSO levelInfoSO)
    {
        sceneLoading.Clear();                                                                               //Clear sceneLoading AsynOperation List
        loadingUIPanel.SetActive(true);                                                                      //Enable Loading Scree
        sceneLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE_SCREEN));                    //Unload Title Screen Scene
        sceneLoading.Add(SceneManager.LoadSceneAsync(levelInfoSO.GetLevelBuildIndex(), LoadSceneMode.Additive));   //Load level scene

        GameManager.Instance.StartCoroutine(GetGameSceneLoadingProgress());
        GameManager.Instance.Resetlevel();
        GameManager.Instance.SetcurrentLevelIndex(levelInfoSO.GetLevelBuildIndex());
        // StartCoroutine(SetupGameReference());
        // StartCoroutine(StartGameCountdown());
    }



    public void RetryLevel()
    {
        sceneLoading.Clear();
        loadingUIPanel.SetActive(true);
        // ResetCurrentGameProgress();
        sceneLoading.Add(SceneManager.UnloadSceneAsync(GameManager.Instance.GetCurrentLevelIndex()));
        sceneLoading.Add(SceneManager.LoadSceneAsync(GameManager.Instance.GetCurrentLevelIndex(), LoadSceneMode.Additive));
        GameManager.Instance.StartCoroutine(GetGameSceneLoadingProgress());
        GameManager.Instance.Resetlevel();
        // StartCoroutine(SetupGameReference());
        // StartCoroutine(StartGameCountdown());
    }

    public IEnumerator GetGameSceneLoadingProgress()
    {
        for (int i = 0; i < sceneLoading.Count; i++)
        {
            while (sceneLoading[i].isDone)
            {
                yield return null;
            }
        }

        Debug.Log("Game Loaded");
        yield return new WaitForSeconds(.1f);
        GameManager.Instance.PrepareGameLevel();
        // GameManager.Instance.SetupGame();
        yield return new WaitForSeconds(1f);
        loadingUIPanel.gameObject.SetActive(false);
        // gameOverScreen.SetActive(false);
    }

    public void LoadGameMenu()
    {
        sceneLoading.Clear();                                                                               //Clear sceneLoading AsynOperation List
        loadingUIPanel.SetActive(true);
        sceneLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.TITLE_SCREEN, LoadSceneMode.Additive));   //Load level scene

        GameManager.Instance.StartCoroutine(GetSceneLoadingProgress());
    }

    public IEnumerator GetSceneLoadingProgress()
    {
        for (int i = 0; i < sceneLoading.Count; i++)
        {
            while (sceneLoading[i].isDone)
            {
                yield return null;
            }
        }
        yield return new WaitForSeconds(1f);
        loadingUIPanel.gameObject.SetActive(false);
    }

    public void LoadNextLevel()
    {
        sceneLoading.Clear();
        loadingUIPanel.SetActive(true);                                                            //Enable Loading Scree

        sceneLoading.Add(SceneManager.UnloadSceneAsync(GameManager.Instance.GetCurrentLevelIndex()));                             //Unload Current level or map
        sceneLoading.Add(SceneManager.LoadSceneAsync(GameManager.Instance.GetNextLevelIndex(), LoadSceneMode.Additive));   //Load next level

        GameManager.Instance.Resetlevel();                                                                           //Reset All previous score to 0

        GameManager.Instance.GetLevelInfoSOList()[GameManager.Instance.GetNextLevelSOIndex()]
                                                              .SetupGameSettings(GameManager.Instance);
            Debug.Log("Load setup Scriptableobject: " + GameManager.Instance.GetLevelInfoSOList()[GameManager.Instance.GetNextLevelSOIndex()].GetLevelName());

        GameManager.Instance.StartCoroutine(GetGameSceneLoadingProgress());
        GameManager.Instance.Resetlevel();
        GameManager.Instance.SetcurrentLevelIndex(GameManager.Instance.GetNextLevelIndex());
    }

    public void RestartGame()
    {
        sceneLoading.Clear();
        loadingUIPanel.SetActive(true);
        // ResetCurrentGameProgress();
        sceneLoading.Add(SceneManager.UnloadSceneAsync(GameManager.Instance.GetCurrentLevelIndex()));
        sceneLoading.Add(SceneManager.LoadSceneAsync(GameManager.Instance.GetCurrentLevelIndex(), LoadSceneMode.Additive));
        GameManager.Instance.StartCoroutine(GetGameSceneLoadingProgress());
        // StartCoroutine(SetupGameReference());
        // StartCoroutine(StartGameCountdown());

    }

    public GameObject GetLoadingGameObject()
    {
        return loadingUIPanel;
    }

}
public enum SceneIndexes { MANAGER = 0, TITLE_SCREEN = 1 }