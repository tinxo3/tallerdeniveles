using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BerserkPromp : MonoBehaviour
{
    [SerializeField] GameObject promp;
    private void OnEnable()
    {
        GameManager.OnComboBarFull +=activatePromp;
        Movement.OnBerserkModeStart += deactivatePromp;
    }

    private void OnDisable()
    {
        GameManager.OnComboBarFull -= activatePromp;
        Movement.OnBerserkModeStart -= deactivatePromp;
    }
    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
    private void activatePromp()
    {
        promp.SetActive(true);
    }
    private void deactivatePromp()
    {
        promp.SetActive(false);
    }
}
