using UnityEngine;

public class LevelInfoButtonHelper : MonoBehaviour
{
    public LevelInfoSO levelInfoScriptablebject;
    [SerializeField] TMPro.TMP_Text levelName_TMP;

    private UnityEngine.UI.Button button;

    private void Awake()
    {
        button = GetComponent<UnityEngine.UI.Button>();
    }

    private void Start()
    {
        levelName_TMP.text = levelInfoScriptablebject.GetLevelName();
        button.onClick.AddListener(delegate { GameManager.Instance.GetGameSceneManager().LoadLevel(levelInfoScriptablebject.GetLevelBuildIndex()); });

    }
}
