using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region PUBLIC_METHODS
    public static Action OnComboBarFull;
    public static Action<int> OnSaveStars;
    public enum MissionsState
    {
        None, First, Medium, Final
    }
    
    [Header("Scene Points")]
    public float scenePoints = 0;
    public float actualPoints = 0;

    [Header("Scene State")]
    [SerializeField] private MissionsState missionsState = MissionsState.None;
    [Range(0, 400)]
    [SerializeField] private int sceneTime = 60;

    [Header("Determine Percentage Missions")]
    [Range(0, 100)]
    [SerializeField] private int firstPercentGoal = 30;
    [Range(0, 100)]
    [SerializeField] private int mediumPercentGoal = 60;
    [Range(0, 100)]
    [SerializeField] private int finalPercentGoal = 100;

    [Header("Player Ref")]
    [SerializeField] private Movement player = null;

    [Header("Combo")]
    [SerializeField] private int targetDestructibles = 7;

    [Header("UI")]
    [SerializeField] private UiGameController uiGameController = null;
    #endregion

    #region PRIVATE_METHODS
    private float firstGoal = 0f;
    private float mediumGoal = 0f;
    private float finalGoal = 0f;

    private float timer = 0f;
    private bool playing = true;
    #endregion

    #region STATIC_CONST_METHODS
    public static float static_scenePoints = 0f;
    private const float PERCENT = 100f;
    #endregion

    #region UNITY_CALLS
    private void Awake()
    {
        missionsState = MissionsState.None;

        timer = sceneTime;

        scenePoints = 0;
        actualPoints = 0;

        player.Init(() => { return uiGameController.ComboBarPlayer.CheckComboBar(); });

        uiGameController.Initialize(onAwakePlayer:() =>
        {
            player.IsMoving = true;
        });
        uiGameController.UpdateTimer(timer);
    }

    private void OnEnable()
    {
        DestructibleComponent.OnDestruction += ChargePoints;
        DestructibleComponent.OnDestruction += ChargeComboBar;
        Movement.OnBerserkModeEnd += UnlockComboBar;
    }

    private void OnDisable()
    {
        static_scenePoints = 0;

        DestructibleComponent.OnDestruction -= ChargePoints;
        DestructibleComponent.OnDestruction -= ChargeComboBar;
        Movement.OnBerserkModeEnd -= UnlockComboBar;
    }

    private void Start()
    {
        scenePoints = static_scenePoints;

        firstGoal = scenePoints * ((float)firstPercentGoal / PERCENT);
        mediumGoal = scenePoints * ((float)mediumPercentGoal / PERCENT);
        finalGoal = scenePoints * ((float)finalPercentGoal / PERCENT);

        Debug.Log(firstPercentGoal + "% percent: " + firstGoal);
        Debug.Log(mediumPercentGoal + "% percent: " + mediumGoal);
        Debug.Log(finalPercentGoal + "% percent: " + finalGoal);

        // UI:
        uiGameController.SetValues(finalGoal, ((float)firstPercentGoal / PERCENT), ((float)mediumPercentGoal / PERCENT), ((float)finalPercentGoal / PERCENT));
    }

    private void Update()
    {
        if(playing && uiGameController.StartGame)
        {
            uiGameController.PauseInput();
            
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timer = 0f;
                EndGame();
            }
            
            if(missionsState == MissionsState.Final)
            {
                EndGame();
            }

            uiGameController.ComboBarPlayer.UpdateGrownState();
            
            uiGameController.UpdateTimer(timer);
        }
    }
    #endregion

    #region PUBLIC_CALLS
    public void ChargePoints(int points)
    {
        actualPoints += points;

        CalculatePercentage();

        uiGameController.UpdateProgressBar(points);
    }

    #endregion

    #region PRIVATE_CALLS
    private void EndGame()
    {
        playing = false;
        player.IsMoving = playing;

        CalculatePercentage();
        int percentage = (int)((actualPoints * 100) / scenePoints);
        SaveManager.singleton.SaveProgress((int)missionsState, percentage, uiGameController.GetActiveSceneIndex());
        OnSaveStars?.Invoke((int)missionsState);
        uiGameController.ActivateMenu(true);
    }

    private void ChargeComboBar(int i)
    {
        uiGameController.ComboBarPlayer.ChargeComboBar(targetDestructibles);
        if (uiGameController.ComboBarPlayer.CheckComboBar())
        {
            OnComboBarFull?.Invoke();
        }
    }

    private void UnlockComboBar()
    {
        uiGameController.ComboBarPlayer.SetDeclineLock(false);
    }

    private void CalculatePercentage()
    {
        if (actualPoints >= firstGoal && missionsState == MissionsState.None)
        {
            uiGameController.OnActivateStar?.Invoke(0);
            missionsState = MissionsState.First;
        }
        
        if (actualPoints >= mediumGoal && missionsState == MissionsState.First)
        {
            uiGameController.OnActivateStar?.Invoke(1);
            missionsState = MissionsState.Medium;
        }
        
        if (actualPoints >= finalGoal && missionsState == MissionsState.Medium)
        {
            uiGameController.OnActivateStar?.Invoke(2);
            missionsState = MissionsState.Final;
        }
    }
    #endregion
}