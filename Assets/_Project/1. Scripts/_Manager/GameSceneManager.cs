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
    [SerializeField] GameObject loadingUIPanel;
    List<AsyncOperation> sceneLoading = new List<AsyncOperation>();

    public void LoadLevel(int buildIndex)
    {
        sceneLoading.Clear();                                                                               //Clear sceneLoading AsynOperation List
        loadingUIPanel.SetActive(true);                                                                      //Enable Loading Scree
        sceneLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE_SCREEN));                    //Unload Title Screen Scene
        sceneLoading.Add(SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive));   //Load level scene

        GameManager.Instance.StartCoroutine(GetSceneLoadingProgress());
        GameManager.Instance.SetcurrentLevelIndex(buildIndex);
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
        GameManager.Instance.StartCoroutine(GetSceneLoadingProgress());
        // StartCoroutine(SetupGameReference());
        // StartCoroutine(StartGameCountdown());
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

        yield return new WaitForSeconds(.1f);
        GameManager.Instance.SetupGame();
        yield return new WaitForSeconds(1f);
        loadingUIPanel.gameObject.SetActive(false);
        // gameOverScreen.SetActive(false);
    }

    public void LoadGame()
    {
        sceneLoading.Clear();                                                                               //Clear sceneLoading AsynOperation List
        loadingUIPanel.SetActive(true);                                                                      //Enable Loading Scree
        sceneLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE_SCREEN));                    //Unload Title Screen Scene
        sceneLoading.Add(SceneManager.LoadSceneAsync(GameManager.Instance.GetCurrentLevelIndex(), LoadSceneMode.Additive));   //Load level scene

        GameManager.Instance.StartCoroutine(GetSceneLoadingProgress());
        // StartCoroutine(SetupGameReference());
        // StartCoroutine(StartGameCountdown());
    }

    public void LoadNextLevel()
    {
        sceneLoading.Clear();
        loadingUIPanel.SetActive(true);                                                                      //Enable Loading Scree
                                                                                                             //Enable Loading Scree
        sceneLoading.Add(SceneManager.UnloadSceneAsync(GameManager.Instance.GetCurrentLevelIndex()));                             //Unload Current level or map
        sceneLoading.Add(SceneManager.LoadSceneAsync(GameManager.Instance.GetCurrentLevelIndex(), LoadSceneMode.Additive));   //Load next level
        // ResetCurrentGameProgress();                                                                             //Reset All previous score to 0
        GameManager.Instance.SetcurrentLevelIndex(GameManager.Instance.GetCurrentLevelIndex());
        // SetMapSettings();                                                                                       //set Map settings, like how many enemy to spawn
        GameManager.Instance.StartCoroutine(GetSceneLoadingProgress());
        // StartCoroutine(StartGameCountdown());
    }

    public void RestartGame()
    {
        sceneLoading.Clear();
        loadingUIPanel.SetActive(true);
        // ResetCurrentGameProgress();
        sceneLoading.Add(SceneManager.UnloadSceneAsync(GameManager.Instance.GetCurrentLevelIndex()));
        sceneLoading.Add(SceneManager.LoadSceneAsync(GameManager.Instance.GetCurrentLevelIndex(), LoadSceneMode.Additive));
        GameManager.Instance.StartCoroutine(GetSceneLoadingProgress());
        // StartCoroutine(SetupGameReference());
        // StartCoroutine(StartGameCountdown());

    }
}
public enum SceneIndexes { MANAGER = 0, TITLE_SCREEN = 1 }