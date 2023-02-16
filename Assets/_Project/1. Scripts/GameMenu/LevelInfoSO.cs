using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LevelInfoSO")]
public class LevelInfoSO : ScriptableObject
{
    [SerializeField] int levelBuildIndex;
    [SerializeField] string levelName;
    [SerializeField] bool isUnlocked;
    [SerializeField] int levelMaxAttempt;
    [SerializeField] DifficulityLevel difficulityLevel;

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

    public string GetLevelName()
    {
        return levelName;
    }

    public bool GetIsUnlocked()
    {
        return isUnlocked;
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

}