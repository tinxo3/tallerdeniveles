using System;
using System.Collections;
using UnityEngine;

public class StatusEffects : MonoBehaviour
{
    public enum StatesEnum
    {
        Burning,
        UpsideDown,
        Trapped
    }

    #region EXPOSED_FIELD
    [SerializeField] private Movement playerMovement = null;
    #endregion
    
    #region PRIVATE_FIELD
    private bool isBurning = false;
    #endregion

    #region PUBLIC_CALLS
    public void BurningState(float time)
    {
        if(isBurning)
        {
            return;
        }

        StartCoroutine(State(time,
        start: () =>
        {
            playerMovement.BurningParticle(true);

            playerMovement.IsMoving = false;
            playerMovement.IsDirectionBlocked = true;
            playerMovement.BurningSFX();

            isBurning = true;
        },
        update: (t) => { },
        end: () =>
        {
            playerMovement.BurningParticle(false);

            playerMovement.IsMoving = true;
            playerMovement.IsDirectionBlocked = false;

            isBurning = false;
        }));
    }

    public void UpsideDownState(float force, Vector3 direction, float time)
    {
        StartCoroutine(State(time,
        start: () =>
        {
            playerMovement.IsMoving = false;
            PlayerThrow(force, direction);
        },
        update: (t) => { },
        end: () =>
        {
            playerMovement.IsMoving = true;
        }));
    }

    public void TrappedState(float time)
    {
        StartCoroutine(State(time, 
        start: () => 
        {
            playerMovement.StopPlayerInertia();
            playerMovement.IsMoving = false;
        },
        update: (t) => { }, 
        end: () => 
        {
            playerMovement.IsMoving = true;            
        }));
    }
    #endregion

    #region PRIVATE_CALLS
    private IEnumerator State(float time, Action start, Action<float> update, Action end)
    {
        float timer = 0;

        start?.Invoke();

        while (timer < time)
        {
            update?.Invoke(timer);

            timer += Time.deltaTime;
            yield return null;
        }

        end?.Invoke();
    }

    private void PlayerThrow(float force, Vector3 direction)
    {
        playerMovement.Rb.velocity = Vector3.zero;
        playerMovement.Rb.AddForce(direction * force, ForceMode.Acceleration);
    }
    #endregion
}
