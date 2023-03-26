using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseBehaviour : MonoBehaviour
{
    #region EXPOSED_METHODS
    [SerializeField] private Button btnContinue = null;
    [SerializeField] private Button btnRestart = null;
    [SerializeField] private Button btnExit = null;
    #endregion

    #region PUBLIC_CALLS
    public void Initialize(Action onContinue, Action onRestart, Action onExit)
    {
        btnContinue.onClick.AddListener(() => { onContinue(); });
        btnRestart.onClick.AddListener(() => { onRestart(); });
        btnExit.onClick.AddListener(() => { onExit(); });
    }

    public void SetActive(bool state)
    {
        this.gameObject.SetActive(state);
    }
    #endregion
}
