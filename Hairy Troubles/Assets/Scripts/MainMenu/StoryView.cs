using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryView : MonoBehaviour
{
    #region EXPOSED_METHODS
    [Header("Story Panel")]
    [SerializeField] private ChangePanelView changePanelView = null;
    [SerializeField] private GameObject skipSign = null;
    [SerializeField] private Animator panelAnimator = null;
    [SerializeField] private List<GameObject> panelsList = new List<GameObject>();

    [Header("Vatiables")]
    [SerializeField] float panelTime = 3f;
    #endregion

    #region PRIVATE_METHODS
    private int pastPanel = 0;
    private int actualPanel = 0;

    private bool storyActivated = false;
    private bool skipState = false;
    #endregion

    #region ACTIONS
    private Action changeScene = null;
    #endregion

    #region UNITY_CALLS
    private void Update()
    {
        if(!storyActivated)
        {
            return;
        }

        if(Input.anyKey && !skipState)
        {
            skipState = true;
            skipSign.SetActive(skipSign);
        }
        else if(Input.GetKeyDown(KeyCode.E) && skipSign)
        {
            changeScene?.Invoke();
        }
    }
    #endregion

    #region PUBLIC_CALLS
    public void Initialize(Action changeScene)
    {
        this.changeScene = changeScene;

        changePanelView.Initialize(ChangePanelStory, () => StartCoroutine(PanelTime()));
    }

    public void StartAnimation()
    {
        panelAnimator.SetBool("Change", true);

        storyActivated = true;
    }

    public void ChangePanelStory()
    {
        if(actualPanel >= panelsList.Count)
        {
            changeScene?.Invoke();
            return;
        }

        if (actualPanel != 0)
        { 
            panelsList[pastPanel].SetActive(false);
        }
        pastPanel = actualPanel;

        panelsList[actualPanel].SetActive(true);
        actualPanel++;
    }
    #endregion

    #region PRIVATE_CALLS
    private IEnumerator PanelTime()
    {
        panelAnimator.SetBool("Change", false);

        yield return new WaitForSecondsRealtime(panelTime);
        
        panelAnimator.SetBool("Change", true);
    }
    #endregion
}