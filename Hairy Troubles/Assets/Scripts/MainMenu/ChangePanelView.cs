using System;
using UnityEngine;

public class ChangePanelView : MonoBehaviour
{
    #region Actions
    private Action changePanel = null;
    private Action panelTime = null;
    #endregion

    #region PUBLIC_METHODS
    public void Initialize(Action onSucces, Action panelTime)
    {
        this.changePanel = onSucces;
        this.panelTime = panelTime;
    }
    
    public void ChangePanel()
    {
        changePanel?.Invoke();
    }
    
    public void PanelTime()
    {
        panelTime?.Invoke();
    }
    #endregion
}
