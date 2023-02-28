using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    private static SaveData _current;

    public static SaveData Current
    {
        get
        {
            if (_current == null)
            {
                _current = new SaveData();
            }
            return _current;
        }
        set
        {
            if (value != null)
            {
                _current = value;
            }
        }
    }

    public PlayerProfile profile;
    public LevelSOData[] levels;
}

[System.Serializable]
public class PlayerProfile
{
    public string playerName;

}

[System.Serializable]
public struct LevelSOData
{
    public string levelName;
    public int starScore;
    public bool isUnlocked;
}
