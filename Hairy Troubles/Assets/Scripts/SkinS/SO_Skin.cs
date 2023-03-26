using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skin", menuName = "Skin")]
public class SO_Skin : ScriptableObject
{
    public enum SkinSlot
    {
        eyes,
        hat,
        body
    }
    public SkinSlot skinSlot;
    public Mesh mesh;
    public Material material;
    public int starsRequired;
}
