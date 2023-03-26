using System;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesWindow : MonoBehaviour
{
    #region PRIVATE_METHODS
    [SerializeField] private Button btnPlay = null;
    #endregion

    //#region UNITY_CALLS
    //private void Awake()
    //{
    //}
    //#endregion

    #region PUBLIC_CALLS
    public void Initialize(Action play)
    {
        gameObject.SetActive(true);

        btnPlay.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            play();
        });
    }
    #endregion
}