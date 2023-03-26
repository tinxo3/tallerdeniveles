using EZCameraController;
using System;
using System.Collections;

using UnityEngine;

public class DestructibleComponent : MonoBehaviour
{
    #region ACTIONS_METHODS
    public static event Action<int> OnDestruction;
    #endregion

    #region EXPOSED_FIELD
    [SerializeField] AudioSource SFXAudio;
    [Header("Mesh To Off")]
    [SerializeField] protected MeshRenderer meshRenderer;

    [Header("Fractured Part")]
    [SerializeField] protected GameObject fracturedObj;
    [SerializeField] protected int points = 10;

    [Header("Particle")]
    [SerializeField] protected GameObject particlePref;

    [Header("Highlight")]
    protected Renderer renderer;
    protected Color startingColor;
    protected Color highlightColor = Color.red;
    [SerializeField] protected float highlightTime = 3f;
    [SerializeField] protected float highlightTimer = 0f;

    [Header("Destruction")]
    [SerializeField] private float destructionTime = 3f;
    [SerializeField] private bool disableColliderWhenDestroy = true;
    [SerializeField] private bool disableRigidbodyWhenDestroy = true;
    #endregion

    #region PRIVATE_FIELD
    protected Rigidbody rig;
    protected Collider meshCollider;
    protected float velocity;

    protected bool isDestroyed = false;
    #endregion

    #region UNITY_CALLS
    protected virtual void Awake()
    {
        renderer = GetComponent<Renderer>();
        
        if(TryGetComponent(out Rigidbody component))
        {
            rig = component;
        }
        if(TryGetComponent(out Collider collider))
        {
            meshCollider = collider;
        }
    }

    private void OnEnable()
    {
        GameManager.static_scenePoints += points;
        Movement.onHighlightRequest += startHighlight;
    }
    private void OnDisable()
    {
        Movement.onHighlightRequest -= startHighlight;
    }
    #endregion

    #region PROTECTED_FIELD
    public void SwapComponent()
    {
        if (!isDestroyed)
        {
            if (SFXAudio != null)
            {
                SFXAudio.Play();
            }
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }
            if (meshCollider != null && disableColliderWhenDestroy)
            {
                meshCollider.enabled = false;
            }
            if (rig != null && disableRigidbodyWhenDestroy)
            {
                //rig.isKinematic = true;
            }

            OnDestruction?.Invoke(points);
            isDestroyed = true;

            CameraController.Instance.ShakeOnce();

            if (fracturedObj != null)
            {
                fracturedObj.SetActive(true);
            }

            if (particlePref != null)
            {
                GameObject go = Instantiate(particlePref, this.transform.position, Quaternion.Euler(Vector3.up));

                Destroy(go, destructionTime);
            }
        }
    }
    #endregion

    #region PRIVATE_FIELD
    private void startHighlight()
    {
        startingColor = renderer.material.GetColor("Color_b9a9dcbfa87e4092934e4e9c37497682");
        renderer.material.SetColor("Color_b9a9dcbfa87e4092934e4e9c37497682",highlightColor);
        StartCoroutine(HighlightItems());
    }

    private IEnumerator HighlightItems()
    {
        while (highlightTimer < highlightTime)
        {
            highlightTimer += Time.deltaTime;
            yield return null;
        }
        highlightTimer = 0.0f;
        renderer.material.SetColor("Color_b9a9dcbfa87e4092934e4e9c37497682", startingColor);
    }
    #endregion

}