using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    #region EXPOSED_METHODS
    [SerializeField] private TMP_Text cdText = null;
    [SerializeField] private Animator animator = null;
    #endregion

    #region PRIVATE_METHODS
    private float cdTimer = 4;
    #endregion

    #region ACTIONS
    private Action onEnd = null;
    #endregion

    #region UNITY_CALLS
    private void Awake()
    {
       gameObject.SetActive(false);
    }
    #endregion

    #region PUBLIC_CALLS
    public void Initialize(Action onEnd)
    {
        this.onEnd = onEnd;
    }

    public void StartCountdown()
    {
        gameObject.SetActive(true);

        animator.SetTrigger("cdNow");
        StartCoroutine(Countdown());
    }
    #endregion

    #region PRIVATE_CALLS
    private IEnumerator Countdown()
    {
        string cdString;
        
        while(cdTimer > 0)
        {
            cdTimer -= Time.unscaledDeltaTime;
            
            if (cdTimer >= 3)
            {
                cdString = "3";
            }
            else if(cdTimer <=0.5)
            {
                cdString = "GO!";
            }
            else
            {
                cdString = cdTimer.ToString("F0");
            }

            cdText.text = cdString;
            
            yield return null;
        }

        onEnd?.Invoke();
    }
    #endregion
}