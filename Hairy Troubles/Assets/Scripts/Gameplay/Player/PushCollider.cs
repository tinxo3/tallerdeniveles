using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushCollider : MonoBehaviour
{
    private float pushTime = 0.25f;
    private float frontforce = 1;
    private float upForce = 1;

    public bool pushing;
    public bool grabbing;

    private Transform parent;
    private Collider coll;

    [Header("-- Berserk Mode --")]
    [SerializeField] private float scaleMultiplier = 2;

    private List<Transform> objectsPushed = new List<Transform>();

    public static Action<Rigidbody> OnObjectGrabbed;
    // ----------------------

    void Awake()
    {
        parent = GetComponentInParent<Transform>();
        coll = GetComponent<Collider>();
    }

    private void Start()
    {
        coll.enabled = false;
    }

    private void OnEnable()
    {
        Movement.IsPushing += CanPush;
        Movement.OnGrab += CanGrab;
        Movement.OnBerserkModeEnd += shrinkColliderSize;
        GameManager.OnComboBarFull += enlargeColliderSize;
    }

    private void OnDisable()
    {
        Movement.IsPushing -= CanPush;
        Movement.OnGrab -= CanGrab;
        Movement.OnBerserkModeEnd -= shrinkColliderSize;
        GameManager.OnComboBarFull -= enlargeColliderSize;
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb;

        other.gameObject.TryGetComponent<Rigidbody>(out rb);

        if (pushing)
        {
            if (rb != null && !objectsPushed.Contains(other.transform))
            {
                objectsPushed.Add(other.transform);
                rb.AddForce(new Vector3(parent.forward.x * frontforce, upForce, parent.forward.z * frontforce), ForceMode.Impulse);
            }
        }
        else if (grabbing)
        {
            Debug.Log("Attempting grab");
            if(rb != null)
            {
                Debug.Log("Found Grabbable");
                OnObjectGrabbed?.Invoke(rb);
            }
            else
            {
                Debug.Log("Failed Grab");
            }
            coll.enabled = false;
        }
        
    }
   
    private void enlargeColliderSize()
    {
        transform.localScale *= scaleMultiplier;
    }
    private void shrinkColliderSize()
    {
        transform.localScale /= scaleMultiplier;
    }

    private void CanGrab()
    {
        Debug.Log("Init grab");
        grabbing = true;
        pushing = false;

        coll.enabled = true;
    }

    private void CanPush(float time, float front, float up)
    {
        pushTime = time;
        frontforce = front;
        upForce = up;

        grabbing = false;
        pushing = true;

        StartCoroutine(PushCountdown());
    }

    IEnumerator PushCountdown()
    {
        coll.enabled = true;

        yield return new WaitForSeconds(pushTime);

        objectsPushed.Clear();
        coll.enabled = false;
    }
}
