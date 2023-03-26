using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    #region EXPOSED_FIELD
    [SerializeField] protected AudioSource SFX;
    [SerializeField] protected Collider hazardCollider = null;
    [SerializeField] [Range(0, 10)] protected float timerEffect = 3f;
    #endregion

    #region UNITY_CALLS
    protected virtual void OnTriggerEnter(Collider other)
    {
        
    }
    #endregion

    #region VIRTUAL_CALLS
    protected virtual void TriggerEvent()
    {

    }
    #endregion

    #region PROTECTED_CALLS
    protected void EnableCollider(bool state)
    {
        hazardCollider.enabled = state;

    }
    #endregion
}