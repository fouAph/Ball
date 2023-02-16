using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameSceneManager
{
    List<AsyncOperation> sceneLoading = new List<AsyncOperation>();

    public void LoadLevel(int buildIndex)
    {
        sceneLoading.Clear();
        sceneLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE_SCREEN));
        sceneLoading.Add(SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive));
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

        yield return new WaitForSeconds(.05f);
        // gameOverScreen.SetActive(false);
        GameManager.Instance.SetupGame();
    }
}
public enum SceneIndexes { MANAGER = 0, TITLE_SCREEN = 1 }