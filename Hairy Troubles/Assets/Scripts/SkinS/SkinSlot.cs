using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SkinSlot : MonoBehaviour
{
    public static SkinSlot singleton;
    enum skinStates
    {
        Locked,
        Unlocked,
        Equipped,
    }
    static public Func<int> OnGetCurrentStars;
    static public Action<SO_Skin> OnSkinEquipped;
    static public Action<SO_Skin> OnSkinUnequipped;
    static public Func<List<SO_Skin>> OnGetSkins;
    [Header("Slotted Skin")]
    [SerializeField] private SO_Skin skin;
    [SerializeField] private TextMeshProUGUI tmp;
    [Header("Buttons")]
    [SerializeField] List<GameObject> buttons;
    [Header("State")]
    [SerializeField] skinStates currentState = skinStates.Locked;

    private void Awake()
    {
        tmp.text = skin.starsRequired.ToString();
    }

    private void OnEnable()
    {
        SkinsManager.OnSkinChange += checkUnequipped;
    }
    private void OnDisable()
    {
        SkinsManager.OnSkinChange -= checkUnequipped;
    }

    private void Start()
    {
        if (OnGetCurrentStars?.Invoke() >= skin.starsRequired)
        {
            currentState = skinStates.Unlocked;
        }
        else
        {
            currentState = skinStates.Locked;
        }
        List<SO_Skin> equippedSkins = OnGetSkins?.Invoke();
        for (int i = 0; i <equippedSkins.Count; i++)
        {
            if(skin == equippedSkins[i])
            {
                currentState = skinStates.Equipped;
            }
        }
        RefreshButtons();
    }

    void checkUnequipped(SO_Skin checkedSkin)
    {
        if (checkedSkin.skinSlot == skin.skinSlot)
        {
            if (checkedSkin != skin)
            {
                currentState = skinStates.Unlocked;
                RefreshButtons();
            }
        }
    }
    void RefreshButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (i != ((int)currentState))
            {
                buttons[i].SetActive(false);
            }
            else
            {
                buttons[i].SetActive(true);
            }
        }
    }
    public void UnequipSkin()
    {
        currentState = skinStates.Unlocked;
        RefreshButtons();
        OnSkinUnequipped?.Invoke(skin);
    }
    public void EquipSkin()
    {
        int currentStars = (int)OnGetCurrentStars?.Invoke();
        if (currentState == skinStates.Unlocked && currentStars >= skin.starsRequired)
        {
            currentState = skinStates.Equipped;
            RefreshButtons();
            OnSkinEquipped?.Invoke(skin);
        }
        else
        {
            //Error sound
        }
    }
}
