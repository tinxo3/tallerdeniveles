using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesLoaderHandler : MonoBehaviour
{
    #region STATIC_CALLS
    const string LOADING_SCENE = "LoadingScreen";
    static string sceneFrom;
    static string sceneToLoad;
    #endregion

    #region EXPOSED_CALLS
    [Header("UI Scene")]
    [SerializeField] private float minimumTime = 5f;
    #endregion

    #region PRIVATE_CALLS
    private float timeLoading;
    private float loadingProgress;

    private float timer = 0f;
    private bool continueScene = false;

    private AsyncOperation operation;
    #endregion

    #region UNITY_CALLS
    private IEnumerator Start()
    {
        yield return SceneManager.UnloadSceneAsync(sceneFrom);
        operation = SceneManager.LoadSceneAsync(sceneToLoad);
        
        loadingProgress = 0;
        timeLoading = 0;

        yield return null;
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            timeLoading += Time.deltaTime;
            loadingProgress = operation.progress + 0.1f;
            loadingProgress = loadingProgress * timeLoading / minimumTime;

            if (loadingProgress >= 1)
            {
                EnableChange();
                break;
            }

            yield return null;
        }
    }

    private void Update()
    {
        if (continueScene == true)
        {
            operation.allowSceneActivation = true;
        }
    }
    #endregion

    #region STATIC_FUNCTIONS
    public static void LoadScene(string scene)
    {
        sceneFrom = SceneManager.GetActiveScene().name;
        sceneToLoad = scene;
        SceneManager.LoadScene(LOADING_SCENE, LoadSceneMode.Additive);
    }
    #endregion

    #region PRIVATE_FUNCTIONS
    private void EnableChange()
    {
        continueScene = true;
    }
    #endregion
}