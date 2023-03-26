using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class UiGameController : MonoBehaviour
{
    #region EXPOSED_FIELD
    [Header("Progress")]
    [SerializeField] private GameObject progressBar = null;

    [Header("Percentage")]
    [SerializeField] private Image percentageBar = null;
    [SerializeField] private TextMeshProUGUI percentageText = null;

    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText = null;

    [Header("Screens")]
    [SerializeField] private string sceneMenu = "MainMenu";
    [SerializeField] private GameObject hubGame = null;

    [Header("Class")]
    [SerializeField] private ObjectivesWindow objectivesWindow = null;
    [SerializeField] private CountdownTimer countdownTimer= null;
    [SerializeField] private PauseBehaviour pauseBehaviour = null;
    [SerializeField] private EndScreenBehaviour endScreenBehaviour = null;
    [SerializeField] private PercentageStarsHolder percentageStarsHolder = null;
    [SerializeField] private ComboBarPlayer comboBarPlayer = null;

    [Header("Transitioner")]
    [SerializeField] private SceneTransition transitioner = null;
    #endregion

    #region PRIVATE_FIELD
    private float currentPercentage = 0f;
    private float maxPercentage = 100f;
    private bool pauseState = false;

    private bool startGame = false;
    #endregion

    #region PROPERTIES
    public ComboBarPlayer ComboBarPlayer { get => comboBarPlayer; }
    public bool StartGame { get => startGame; }
    #endregion

    #region ACTIONS
    public Action OnPlayButton = null;
    public Action<int> OnActivateStar = null;
    #endregion

    #region UNITY_CALLS
    private void OnEnable()
    {
        GameManager.OnComboBarFull += ChangeBerserkUI;
        Movement.OnBerserkModeEnd += ChangeBerserkUI;
        OnPlayButton += countdownTimer.StartCountdown;
        OnActivateStar += endScreenBehaviour.ActivateFinalStars;
    }

    private void OnDisable()
    {
        GameManager.OnComboBarFull -= ChangeBerserkUI;
        Movement.OnBerserkModeEnd -= ChangeBerserkUI;
        OnPlayButton -= countdownTimer.StartCountdown;
        OnActivateStar -= endScreenBehaviour.ActivateFinalStars;
    }
    #endregion

    #region PUBLIC_CALLS
    public void Initialize(Action onAwakePlayer)
    {
        hubGame.SetActive(false);

        objectivesWindow.Initialize(() => { 
            OnPlayButton?.Invoke();
            hubGame.SetActive(true);
        });

        countdownTimer.Initialize(onEnd: () => 
        {
            startGame = true;
            onAwakePlayer?.Invoke();
        });
        pauseBehaviour.Initialize(DisablePause, OnRestartScene, OnGoToScene);
        endScreenBehaviour.Initialize(OnRestartScene, OnGoToScene);
    }

    public void SetValues(float finalGoal, float firstPercentGoal, float mediumPercentGoal, float finalPercentGoal)
    {
        SetMaximumProgress(finalGoal);
        percentageStarsHolder.Initialize(firstPercentGoal, mediumPercentGoal, finalPercentGoal);
    }

    public void PauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && startGame)
        {
            DisablePause();
        }
    }

    public void UpdateTimer(float time)
    {
        timerText.text = time.ToString("0");
    }

    public void ActivateMenu(bool state)
    {
        endScreenBehaviour.SetActive(state);
        hubGame.SetActive(!state);
    }
    public int GetActiveSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
    public void SetMaximumProgress(float value)
    {
        maxPercentage = value;
    }

    public void UpdateProgressBar(float current)
    {
        currentPercentage += current;
        percentageBar.fillAmount = currentPercentage / maxPercentage;

        percentageText.text = "%" + (percentageBar.fillAmount * 100).ToString("0");

        endScreenBehaviour.SetFinalPercentage(percentageText.text);
    }
    #endregion

    #region PRIVATE_CALLS

    private void ChangeBerserkUI()
    {
        ComboBarPlayer.changeBerserkUI();
    }
    private void DisablePause()
    {
        pauseState = !pauseState;

        if (!pauseState)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }

        pauseBehaviour.SetActive(pauseState);
    }

    private void OnRestartScene()
    {
        if (pauseState) DisablePause();

        transitioner.ChangeAnimation(1, () =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }

    private void OnGoToScene()
    {
        if (pauseState) DisablePause();

        transitioner.ChangeAnimation(1, () =>
        {
            SceneManager.LoadScene(sceneMenu);
        });
    }
    #endregion
}