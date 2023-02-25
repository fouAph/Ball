using UnityEngine;
using UnityEngine.UI;
public class LevelInfoButtonHelper : MonoBehaviour
{

    public LevelInfoSO levelInfoScriptablebject;
    [SerializeField] Transform starsContainer;
    [SerializeField] Sprite emptyStarSprite;
    [SerializeField] Sprite filledStarSprite;

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
            GameManager.Instance.GetGameSceneManager().LoadGameLevel(levelInfoScriptablebject);
            GameManager.Instance.GetLevelInfoSOList()[GameManager.Instance.GetCurrentLevelSOIndex()].SetupGameSettings(GameManager.Instance);
            GameManager.Instance.Resetlevel();
        });

        button.interactable = levelInfoScriptablebject.GetIsUnlocked();
    }

    public void SpawnStars()
    {
        int count = 0;
        for (int i = 0; i < starsContainer.childCount; i++)
        {
            Image img = starsContainer.GetChild(i).GetComponent<Image>();
            if (count < levelInfoScriptablebject.GetStarScore())
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
