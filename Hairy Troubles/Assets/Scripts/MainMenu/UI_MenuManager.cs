using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MenuManager : MonoBehaviour
{
    #region EXPOSED_CALLS
    [Header("Transitioner")]
    [SerializeField] private SceneTransition sceneTransition = null;
    [SerializeField] private StoryView storyView = null;

    [Header("Buttons")]
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnExit;

    [Header("Scene To Go")]
    [SerializeField] private string sceneName = "LoadingScreen";
    #endregion

    #region UNITY_CALLS
    private void Awake()
    {
        storyView.Initialize(OnLoadLevel);

        btnPlay.onClick.AddListener(storyView.StartAnimation);
        btnExit.onClick.AddListener(OnExitGame);
    }
    #endregion

    #region PUBLIC_CALLS
    public void OnLoadLevel()
    {
        btnPlay.onClick = null;
        btnExit.onClick = null;

        sceneTransition.ChangeAnimation(1, ()=> { 
            ScenesLoaderHandler.LoadScene(sceneName);
        });
    }

    public void OnExitGame()
    {
        Application.Quit();
    }
    #endregion
}