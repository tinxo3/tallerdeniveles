using System;
using UnityEngine;
using TMPro;
public class UI_CurrentStars : MonoBehaviour
{
    static public Func<int> OnGetCurrentStars;
    TextMeshProUGUI text;
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = SaveManager.singleton.data._stars.ToString();
    }
}
