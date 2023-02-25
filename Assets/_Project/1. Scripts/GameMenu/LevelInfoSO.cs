using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LevelInfoSO")]
public class LevelInfoSO : ScriptableObject
{
    [SerializeField] int levelBuildIndex;
    [SerializeField] string levelName;

    [Header("Game Settings")]
    [SerializeField] float maxDragDistance = 3;
    [SerializeField] int levelMaxAttempt = 3;
    [SerializeField] float pushForce;
    [SerializeField] bool isUnlocked;
    [SerializeField] DifficulityLevel difficulityLevel;
    [SerializeField] int starScore = 0;
    private void OnEnable()
    {
        if (this.name != levelName)
        {
            Debug.Log("Change name");
            levelName = this.name;
        }
    }

    public int GetLevelBuildIndex()
    {
        return levelBuildIndex;
    }

    public void SetupGameSettings(GameManager gameManager)
    {
        gameManager.gameSettings.maxDragDistance = maxDragDistance;
        gameManager.gameSettings.maxAttempt = levelMaxAttempt;
        gameManager.gameSettings.pushForce = pushForce;
    }

    public void SetLevelBuildIndex(int index)
    {
        levelBuildIndex = index;
    }

    public string GetLevelName()
    {
        return levelName;
    }

    public bool GetIsUnlocked()
    {
        return isUnlocked;
    }

    public void SetIsUnlocked(bool unlock)
    {
        isUnlocked = unlock;
    }

    public int GetLevelMaxAttempt()
    {
        return levelMaxAttempt;
    }

    public string GetDifficulityLevelString()
    {
        return difficulityLevel.ToString();
    }

    public int GetDifficulityLevelInt()
    {
        return (int)difficulityLevel;
    }

    private enum DifficulityLevel { EASY, MEDIUM, HARD }

    public int GetStarScore()
    {
        return starScore;
    }

    public void SetStarScore(int score)
    {
        starScore = score;
    }
}
