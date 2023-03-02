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

    [SerializeField] GameObject pauseGamePopup;
    [SerializeField] GameObject settingsGamePopup;
    [Header("Pause Menu's Buttons")]
    [SerializeField] Button pauseButton;
    [SerializeField] Button resumeButton;
    [SerializeField] Button restartButton;
    [SerializeField] Button settingButton;
    [SerializeField] Button mainMenuButton;


    private void Start()
    {
        HideLevelWinPopup();
        HideLevelFailedPopup();

        GameManager.Instance.onGameLose += ShowGameOverPopup_OnGameLose;
        GameManager.Instance.onGameWin += ShowGameOverPopup_OnGameWin;

        pauseButton.onClick.AddListener(() => PauseButton_OnButtonClick());
        resumeButton.onClick.AddListener(() => PauseButton_OnButtonClick());

        restartButton.onClick.AddListener(() => GameManager.Instance.Retry_OnButtonClick());
        mainMenuButton.onClick.AddListener(() => GameManager.Instance.MainMenu_OnButtonClick());
        settingButton.onClick.AddListener(() => SettingButton_OnButtonClick());
    }

    private void OnDisable()
    {
        GameManager.Instance.onGameLose -= ShowGameOverPopup_OnGameLose;
        GameManager.Instance.onGameWin -= ShowGameOverPopup_OnGameWin;
    }

    [NonSerialized] bool pauseGamePopupisOpen = false;
    public void PauseButton_OnButtonClick()
    {
        pauseGamePopupisOpen = !pauseGamePopupisOpen;
        pauseGamePopup.SetActive(pauseGamePopupisOpen);

    }

    [NonSerialized] bool settingsGamePopupisOpen;
    public void SettingButton_OnButtonClick()
    {
        settingsGamePopupisOpen = !settingsGamePopupisOpen;
        settingsGamePopup.SetActive(settingsGamePopupisOpen);
    }

    private void Retry_OnButtonClick()
    {
        GameManager.Instance.Retry_OnButtonClick();
    }

    private void MainMenu_OnButtonClick()
    {
        GameManager.Instance.MainMenu_OnButtonClick();
    }

    private void NextLevel_OnButtonClick()
    {
        GameManager.Instance.NextLevel_OnButtonClick();
    }

    private void ShowGameOverPopup_OnGameLose(object sender, EventArgs e)
    {
        HideLevelWinPopup();
        ShowLevelFailedPopup();
        SpawnStars();
    }

    private void ShowGameOverPopup_OnGameWin(object sender, EventArgs e)
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
            if (count < GameManager.Instance.GetStarsResult())
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

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] float masterVolume = 1f;
    [SerializeField] SFXManager sfxManager;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySoundEffect(AudioClip sfxClip, float volume = 1f)
    {
        sfxManager.PlaySoundEffect(sfxClip, volume);
    }

    public void PlaySoundEffect(AudioSource source, AudioClip sfxClip, float volume = 1f)
    {
        sfxManager.PlaySoundEffect(source, sfxClip, volume);
    }

    [System.Serializable]
    private class SFXManager
    {
        public AudioSource soundEffectSource;
        [SerializeField] float sfxVolume;
        public void PlaySoundEffect(AudioClip sfxClip, float volume = 1f)
        {
            soundEffectSource.volume = volume * sfxVolume * AudioManager.Instance.masterVolume;
            soundEffectSource.PlayOneShot(sfxClip);
        }

        public void PlaySoundEffect(AudioSource source, AudioClip sfxClip, float volume = 1f)
        {
            source.volume = volume * sfxVolume * AudioManager.Instance.masterVolume;
            source.PlayOneShot(sfxClip);
        }
    }
}