using System;
using System.Collections.Generic;
using UnityEngine;

public class SkinsManager : MonoBehaviour
{
    public static SkinsManager singleton;
    static public Action OnSendCurrentStars;
    static public Action<SO_Skin> OnSkinChange;
    [Header("Stars")]
    [SerializeField] private int currentStars;
    [Header("Skins")]
    [SerializeField] private SO_Skin currentEyesSkin;
    [SerializeField] private SO_Skin currentHatSkin;
    [SerializeField] private SO_Skin currentBodySkin;
    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        PlayerSkin.OnRecieveSkin += LoadSelectedSkins;
        GameManager.OnSaveStars += SaveStars;
        SkinSlot.OnGetSkins += LoadSelectedSkins;
        SkinSlot.OnGetCurrentStars += SendStars;
        SkinSlot.OnSkinEquipped += SkinChange;
    }
    private void OnDisable()
    {
        PlayerSkin.OnRecieveSkin -= LoadSelectedSkins;
        GameManager.OnSaveStars -= SaveStars;
        SkinSlot.OnGetCurrentStars -= SendStars;
        SkinSlot.OnSkinEquipped -= SkinChange;
    }
    private void Start()
    {
        currentStars = SaveManager.singleton.data._stars;
    }
    int SendStars()
    {
        return currentStars;
    }

    List<SO_Skin> LoadSelectedSkins()
    {
        List<SO_Skin> skins = new List<SO_Skin>();
        skins.Add(currentEyesSkin);
        skins.Add(currentHatSkin);
        skins.Add(currentBodySkin);
        return skins;
    }

    void SkinChange(SO_Skin skin)
    {
        switch (skin.skinSlot)
        {
            case SO_Skin.SkinSlot.eyes:
                if(skin == currentEyesSkin)
                {
                    currentEyesSkin = null;
                }
                else
                {
                    currentEyesSkin = skin;
                }
                break;
            case SO_Skin.SkinSlot.hat:
                if (skin == currentHatSkin)
                {
                    currentHatSkin = null;
                }
                else
                {
                    currentHatSkin = skin;
                }
                break;
            case SO_Skin.SkinSlot.body:
                if (skin == currentBodySkin)
                {
                    currentBodySkin = null;
                }
                else
                {
                    currentBodySkin = skin;
                }
                break;
        }
        OnSkinChange?.Invoke(skin);
    }
    void SaveStars(int savedStars)
    {
        currentStars += savedStars;
    }
}
