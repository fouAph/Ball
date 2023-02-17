using UnityEngine;
using UnityEngine.UI;
public class LevelInfoButtonHelper : MonoBehaviour
{
    public LevelInfoSO levelInfoScriptablebject;
    [SerializeField] Color lockedSpriteColor;
    [SerializeField] GameObject lockedImageGameobject;
    [SerializeField] TMPro.TMP_Text levelName_TMP;

    private Color defaultColor;
    private Button button;
    private Image buttonImage;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        
        defaultColor = buttonImage.color;
        if (!levelInfoScriptablebject.GetIsUnlocked())
        {
            buttonImage.color = lockedSpriteColor;
            lockedImageGameobject.SetActive(true);
        }

        else
        {
            buttonImage.color = defaultColor;
            lockedImageGameobject.SetActive(false);
        }
    }


    private void Start()
    {
        levelName_TMP.text = levelInfoScriptablebject.GetLevelName();
        button.onClick.AddListener(delegate
        {
            GameManager.Instance.GetGameSceneManager().LoadLevel(levelInfoScriptablebject.GetLevelBuildIndex());
            GameManager.Instance.Resetlevel();
        });

        button.interactable = levelInfoScriptablebject.GetIsUnlocked();
    }
}
