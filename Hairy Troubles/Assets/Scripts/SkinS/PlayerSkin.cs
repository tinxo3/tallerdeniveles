using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    [Header("Skin Slots")]
    [SerializeField] List<MeshFilter> skinSlotsMesh;
    [SerializeField] List<MeshRenderer> skinSlotsMaterial;
    public static Func<List<SO_Skin>> OnRecieveSkin;

    private void Start()
    {
        ApplySkins(OnRecieveSkin?.Invoke());
    }

    void ApplySkins(List<SO_Skin> skins)
    {
        if(skins != null)
        {
            for (int i = 0; i < skins.Count; i++)
            {
                if (skins[i] != null)
                {
                    skinSlotsMesh[i].mesh = skins[i].mesh;
                    skinSlotsMaterial[i].material = skins[i].material;
                }
                else
                {
                    skinSlotsMesh[i].mesh = null;
                    skinSlotsMaterial[i].material = null;
                }
            }
        }
    }
}
