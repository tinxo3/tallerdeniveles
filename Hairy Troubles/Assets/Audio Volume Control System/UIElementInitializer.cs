using UnityEngine;
using UnityEngine.UI;

public class UIElementInitializer : MonoBehaviour
{
    public enum UIElementType { 
       SFX_Slider,
       MUSIC_Slider
    }

    public UIElementType type;

    Slider slider;

    private void Start()
    {
        switch (type)
        {
            case UIElementType.SFX_Slider:
                slider = GetComponent<Slider>();
                slider.value = AudioSettings.audioSettings.GetSFXVolume();
                break;
            case UIElementType.MUSIC_Slider:
                slider = GetComponent<Slider>();
                slider.value = AudioSettings.audioSettings.GetMusicVolume();
                break;
        }

    }

}
