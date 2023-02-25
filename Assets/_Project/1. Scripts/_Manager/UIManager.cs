using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] GameHUD gameHUD;
    [SerializeField] GameObject levelWinPopup;
    [SerializeField] GameObject levelFailedPopup;

    [SerializeField] Transform starsContainer;
    [SerializeField] Sprite emptyStarSprite;
    [SerializeField] Sprite filledStarSprite;

    [SerializeField] TMPro.TMP_Text gameOverTxt;
    [SerializeField] string[] winTexts;
    [SerializeField] string[] loseTexts;

    private void Start()
    {
        HideLevelWinPopup();
        HideLevelFailedPopup();

        GameManager.Instance.onGameLose += ShowGameOverPopup_OnGameLose;
        GameManager.Instance.onGameWin += ShowGameOverPopup_OnGameWin;

        // retryButton.onClick.AddListener(() => GameManager.Instance.Retry_OnButtonClick());
        // mainMenuButton.onClick.AddListener(() => GameManager.Instance.MainMenu_OnButtonClick());
        // nextLevelButton.onClick.AddListener(() => GameManager.Instance.NextLevel_OnButtonClick());
    }

    private void OnDisable()
    {
        GameManager.Instance.onGameLose -= ShowGameOverPopup_OnGameLose;
        GameManager.Instance.onGameWin -= ShowGameOverPopup_OnGameWin;
    }

    public void Retry_OnButtonClick()
    {
        GameManager.Instance.Retry_OnButtonClick();
    }

    public void MainMenu_OnButtonClick()
    {
        GameManager.Instance.MainMenu_OnButtonClick();
    }

    public void NextLevel_OnButtonClick()
    {
        GameManager.Instance.NextLevel_OnButtonClick();
    }

    public void ShowGameOverPopup_OnGameLose(object sender, EventArgs e)
    {
        HideLevelWinPopup();
        ShowLevelFailedPopup();
        SpawnStars();
    }

    public void ShowGameOverPopup_OnGameWin(object sender, EventArgs e)
    {
        HideLevelFailedPopup();
        ShowLevelWinPopup();
        SpawnStars();
    }

    private void ShowLevelWinPopup()
    {

        levelWinPopup.SetActive(true);
        levelWinPopup.transform.parent.gameObject.SetActive(true);
    }

    private void HideLevelWinPopup()
    {

        levelWinPopup.SetActive(false);
        levelWinPopup.transform.parent.gameObject.SetActive(false);
    }

    private void ShowLevelFailedPopup()
    {
        levelFailedPopup.SetActive(true);
        levelFailedPopup.transform.parent.gameObject.SetActive(true);
    }

    private void HideLevelFailedPopup()
    {
        levelFailedPopup.SetActive(false);
        levelFailedPopup.transform.parent.gameObject.SetActive(false);
    }

    private void SpawnStars()
    {
        int count = 0;
        for (int i = 0; i < starsContainer.childCount; i++)
        {
            Image img = starsContainer.GetChild(i).GetComponent<Image>();
           if (count <  GameManager.Instance.GetStarsResult())
            {
                img.sprite = filledStarSprite;
            }

            else
            {
                img.sprite = emptyStarSprite;
            }
  
            count++;
        }
    }
}