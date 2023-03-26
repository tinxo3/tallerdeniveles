using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_LevelController : MonoBehaviour
{
    const int levelArrayOffset = 1;
    #region EXPOSED_FIELDS
    [SerializeField] private SO_Level level;

    [Header("In Prefab Obj")]
    [SerializeField] private GameObject panel = null;
    [SerializeField] private List<Image> Stars = null;
    [SerializeField] private TextMeshProUGUI progress = null;
    [SerializeField] private GameObject marker = null;
    [SerializeField] private GameObject right = null;
    [SerializeField] private GameObject left = null;
    [SerializeField] private GameObject flagsHolder = null;
    [SerializeField] private Image flag = null;
    [SerializeField] private Image brokenFlag = null;

    [Header("In Scene Obj")]
    [SerializeField] private Image house = null;
    [SerializeField] private Image brokenHouse = null;
    #endregion

    #region UNITY_CALLS
    private void OnEnable()
    {
        LevelSelectionManager.OnStart += UpdateUI;
        LevelSelectionManager.OnStart += ChangePanel;
        LevelSelectionManager.OnStart += ChangeMarker;
        LevelSelectionManager.OnLevelchange += UpdateUI;
        LevelSelectionManager.OnLevelchange += ChangePanel;
        LevelSelectionManager.OnLevelchange += ChangeMarker;
    }

    private void OnDisable()
    {
        LevelSelectionManager.OnStart -= UpdateUI;
        LevelSelectionManager.OnStart -= ChangePanel;
        LevelSelectionManager.OnStart -= ChangeMarker;
        LevelSelectionManager.OnLevelchange -= UpdateUI;
        LevelSelectionManager.OnLevelchange -= ChangePanel;
        LevelSelectionManager.OnLevelchange -= ChangeMarker;
    }
    #endregion

    #region PRIVATE_CALLS
    private void ChangePanel(SO_Level currentLevel, HairyTroublesData data)
    {
        if (level == currentLevel)
        {
            panel.SetActive(true);
            flagsHolder.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
            flagsHolder.SetActive(false);
        }
    }

    private void ChangeMarker(SO_Level currentLevel, HairyTroublesData data)
    {
        if (level == currentLevel)
        {
            if(data._levelClear[currentLevel.levelNumber - levelArrayOffset])
            {
                right.SetActive(true);
            }
            else
            {
                right.SetActive(false);
            }
            int index = (currentLevel.levelNumber - 1 - levelArrayOffset) % data._levelClear.Length;
            if (index >= 0 && data._levelClear[index])
            {
                left.SetActive(true);
            }
            else
            {
                left.SetActive(false);
            }
        }
        else
        {
            right.SetActive(false);
            left.SetActive(false);
        }
    }

    private void UpdateUI(SO_Level currentLevel, HairyTroublesData data)
    {
        int index = level.levelNumber - levelArrayOffset % data._levelClear.Length;
        
        if (data._levelClear[index])
        {
            brokenHouse.enabled = true;
            house.enabled = false;
            brokenFlag.enabled = true;
            flag.enabled = false;
        }
        else
        {
            brokenHouse.enabled = false;
            house.enabled = true;
            brokenFlag.enabled = false;
            flag.enabled = true;
        }

        progress.text = data._levelProgress[index].ToString()+"%";
        for(int i = 0; i < data._levelStars[index]; i++)
        {
            Stars[i].enabled = true;
        }
    }
    #endregion
}