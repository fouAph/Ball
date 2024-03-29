using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    [SerializeField] Menu[] menus;

    [SerializeField] string currentMenu;
    [SerializeField] string previousMenu;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one Instance of " + Instance.GetType());
            Debug.LogWarning("Destroying " + Instance.name);
            DestroyImmediate(Instance.gameObject);
        }

        Instance = this;
    }

    [SerializeField] AudioClip selectMenuClip;
    [SerializeField] AudioClip exitSelectMenuClip;
    [SerializeField] AudioClip clickMenuClip;

    [Space(10)]
    [SerializeField] Transform levelButtonPrefab;
    [SerializeField] Transform levelSelectContainer;
 
    [SerializeField] TMPro.TMP_Dropdown dropdown;
    // AudioPoolSystem audioPoolSystem;

    private void Start()
    {
        OpenMenu("main");
        List<string> strings = new List<string>();
        dropdown.onValueChanged.AddListener(delegate { ApplyFPSTargetSetting(dropdown); });
        
#if UNITY_EDITOR

        return;
#endif
#if !UNITY_EDITOR
        ApplyFPSTargetSetting(dropdown);
#endif

    }

    private void OnEnable()
    {
        SpawnLevelButton();
    }

    public void SpawnLevelButton()
    {
        for (int i = 0; i < GameManager.Instance.GetLevelInfoSOList().Length; i++)
        {
            //if an error directing to this script, then Disable LevelSelect_Menu Gameobject on MainMenu Scene inside Canvas 
            var go = Instantiate(levelButtonPrefab, levelSelectContainer);
            LevelInfoButtonHelper info = go.GetComponent<LevelInfoButtonHelper>();
            info.levelInfoScriptablebject = GameManager.Instance.GetLevelInfoSOList()[i];
            info.levelInfoScriptablebject.SetLevelBuildIndex(i + 2);
            info.SpawnStars();
        }
    }

    public void OnSelectMenu()
    {
        // if (audioPoolSystem && selectMenuClip)
        {
            // audioPoolSystem.PlayAudioMenu(selectMenuClip, 1f);
        }
    }

    public void OnExitSelectMenu()
    {
        // if (audioPoolSystem && exitSelectMenuClip)
        {
            // audioPoolSystem.PlayAudioMenu(exitSelectMenuClip, 1f);
        }
    }

    public void OnClickMenu()
    {
        // if (audioPoolSystem && clickMenuClip)
        {
            // audioPoolSystem.PlayAudioMenu(clickMenuClip, 1f);
        }
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                previousMenu = currentMenu;
                menus[i].Open();
                currentMenu = menus[i].menuName;
            }
            else
            {
                menus[i].Close();
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menu.menuName)
            {
                previousMenu = currentMenu;
                menus[i].Open();
                currentMenu = menus[i].menuName;
            }
            else
            {
                menus[i].Close();
            }
        }
    }

    public void OnBackButton_Click()
    {
        OpenMenu(previousMenu);
    }

    public void ApplyFPSTargetSetting(TMPro.TMP_Dropdown dropdown)
    {
        switch (dropdown.value)
        {
            case 0:
                Application.targetFrameRate = 60;
                break;

            case 1:
                Application.targetFrameRate = 30;
                break;
        }
    }

    public void ApplyMasterVolumeSetting()
    {

    }

    public void ApplyMusicSetting()
    {

    }
}
 