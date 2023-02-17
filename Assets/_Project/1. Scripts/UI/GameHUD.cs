using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHUD : MonoBehaviour
{
    [SerializeField] Transform ballLifeUIContainer;
    [SerializeField] Transform ballLifeUIPrefab;

    private GameManager gameManager;

    private void OnDisable()
    {
        gameManager.onCurrentAttemptCountChange -= UpdateBallsLifeUI_OnCurrentAttemptCountChange;
    }
    
    private void Start()
    {
        gameManager = GameManager.Instance;

        gameManager.onCurrentAttemptCountChange += UpdateBallsLifeUI_OnCurrentAttemptCountChange;

        for (int i = 0; i < gameManager.GetMaxAttempt(); i++)
        {
            Instantiate(ballLifeUIPrefab, ballLifeUIContainer);
        }
    }

    private void UpdateBallsLifeUI_OnCurrentAttemptCountChange(object sender, EventArgs e)
    {
        for (int i = 0; i < gameManager.GetMaxAttempt(); i++)
        {
            ballLifeUIContainer.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < gameManager.GetCurrentAttemptCount(); i++)
        {
            ballLifeUIContainer.GetChild(i).gameObject.SetActive(true);
        }
    }
}
