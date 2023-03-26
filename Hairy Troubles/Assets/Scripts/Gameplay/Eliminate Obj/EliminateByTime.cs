using System.Collections;
using UnityEngine;

public class EliminateByTime : MonoBehaviour
{
    public enum Type
    {
        Furniture,
        Ornaments
    }

    #region PUBLIC_FIELD
    [SerializeField] private Type objectType;
    [SerializeField] private GameObject[] fragments;

    [Header("Force Push Options")]
    [SerializeField] private Vector3 direction = Vector3.zero;
    [SerializeField] private float force = 0f;
    #endregion

    #region PRIVATE_METHODS
    private float secondsToDestroy = 10f;
    private float speedAnimation = 1f;

    private bool animate = true;
    private float time = 0f;
    #endregion

    #region UNITY_CALLS
    private void OnEnable()
    {
        secondsToDestroy = Control_Obj_Eliminations.Get().GetSeconds(objectType);
        speedAnimation = Control_Obj_Eliminations.Get().GetSpeed(objectType);
        
        animate = false;
    }

    private void FixedUpdate()
    {
        IEnumerator DestroyAnimation()
        {
            float time = 0f;

            Vector3 oneScale = Vector3.one;
            Vector3 zeroScale = Vector3.zero;

            while (time <= 1f)
            {
                time += Time.deltaTime;

                for (int i = 0; i < fragments.Length; i++)
                {
                    fragments[i].transform.localScale = Vector3.Lerp(oneScale, zeroScale, time);
                }

                yield return null;
            }

            Destroy(this.gameObject);
        }

        if (!animate)
        {
            time += Time.deltaTime;

            if (time > secondsToDestroy)
            {
                animate = true;

                if(fragments.Length > 0)
                {
                    if(force > 0)
                    {
                        for (int i = 0; i < fragments.Length; i++)
                        {
                            fragments[i].GetComponent<Rigidbody>().AddForce(direction * force, ForceMode.Impulse);
                        }
                    }

                    StartCoroutine(DestroyAnimation());
                }
            }
        }
    }
    #endregion

    #region PUBLIC_CALLS
    public void SetSecondsToDestroy(float seconds)
    {
        secondsToDestroy = seconds;
    }

    public void SetSpeedAnimation(float speed)
    {
        speedAnimation = speed;
    }

    public float GetSecondsToDestroy(float seconds)
    {
        return secondsToDestroy;
    }

    public float GetSpeedAnimation(float speed)
    {
        return speedAnimation ;
    }
    #endregion
}