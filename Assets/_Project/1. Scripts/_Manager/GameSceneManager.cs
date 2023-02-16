using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    List<AsyncOperation> sceneLoading = new List<AsyncOperation>();
    
    public void LoadLevel(int buildIndex)
    {
        SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
    }
}