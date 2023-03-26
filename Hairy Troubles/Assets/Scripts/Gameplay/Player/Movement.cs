using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movement : MonoBehaviour, ICollidable
{
    #region EXPOSED_METHODS
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource steps;
    [SerializeField] private List<AudioClip> sfxsClips;
    enum SfxName
    {
        jump,
        steps,
        fireInjure,
        atack
    }
    SfxName sfxName;
    [Space(10f)]
    [Header("-- Movement --")]
    [SerializeField] private float movementSpeed = 1.17f;
    [SerializeField] private float smoothRotation = 0.2f;
    [SerializeField] private float jumpforce = 14;
    [SerializeField] private float pushforce = 7;
    [SerializeField] private float pushCooldown = 1;
    [SerializeField] private float pushCountdown = 0;
    [SerializeField] private float positionY = 0;
    [SerializeField] private List<Transform> raycast;
    [Space(10f)]
    
    [Header("-- Push --")]
    [Space(20f)]
    [Range(0.01f, 1f)]
    [SerializeField] private float pushTime = 0.25f;
    [SerializeField] private float frontForce = 20;
    [SerializeField] private float upForce = 30;

    [Space(10f)]
    [Header("-- Grab --")]
    [SerializeField] private float springForce = 100000000;
    [SerializeField] private float dragForce = 10;
    [SerializeField] private float pushDragThreshold = 20f;
    [SerializeField] private Transform anchorPoint;

    [Space(10f)]
    [Header("-- Berserk Mode --")]
    [SerializeField] private Renderer bodyRend;
    [SerializeField] private Renderer eyesRend;
    [SerializeField] private float duration;
    [SerializeField] private float jumpForceBuff = 1;
    [SerializeField] private Color tint;

    [Header("-- Particles --")]
    [SerializeField] private ParticleSystem fireParticle = null;
    [SerializeField] private ParticleSystem dustTrail = null;
    [SerializeField] private ParticleSystem slamDustTrail = null;
    #endregion

    #region PRIVATE_METHODS
    private Rigidbody rb;

    private Vector3 movementDirection;
    private float hor;
    private float ver;
    private float yVelocity;
    
    private bool invertMovement;
    private bool canJump = true;
    private bool isMoving = false;
    private bool isDirectionBlocked = false;
    private bool berserkMode;
    #endregion

    #region ACTIONS
    public static Action<float, float, float> IsPushing;
    public static Action OnGrab;
    public static Action onHighlightRequest;
    public static Action OnBerserkModeStart;
    public static Action OnBerserkModeEnd;
    public Func<bool> OnHableToActivateBerserk;
    #endregion

    #region PROPERTIES
    public bool IsMoving { get => isMoving; set => isMoving = value; }
    public bool IsDirectionBlocked { get => isDirectionBlocked; set => isDirectionBlocked = value; }
    public Rigidbody Rb { get => rb; }
    #endregion

    #region UNITY_CALLS
    private void OnEnable()
    {
        BurningParticle(false);
        GameManager.OnComboBarFull += EnableBerserkMode;
        PushCollider.OnObjectGrabbed += RecieveGrabbed;
    }

    private void OnDisable()
    {
        GameManager.OnComboBarFull -= EnableBerserkMode;
        PushCollider.OnObjectGrabbed -= RecieveGrabbed;
    }
    private void Start()
    {
        
    }
    void Update()
    {
        if (isMoving)
        {
            hor = Input.GetAxis("Horizontal");
            ver = Input.GetAxis("Vertical");

            movementDirection = new Vector3(hor, 0, ver);
            movementDirection.Normalize();

            PlayerJumpLogic();

            PlayerHighlightRequest();

            PlayerGrabLogic();

            PlayerPushLogic();

            EnterBerserkMode();
        }
    }
    
    private void FixedUpdate()
    {
        if (isMoving)
        {
            PlayerMovement();
        }
        else if (isDirectionBlocked)
        {
            BlockedMovement();
        }
    }
    #endregion

    #region PUBLIC_CALLS
    public void Init(Func<bool> OnHableToActivateBerserk)
    {
        rb = GetComponent<Rigidbody>();

        this.OnHableToActivateBerserk = OnHableToActivateBerserk;

        isMoving = false;
    }

    public void StopCharacter(bool state)
    {
        isMoving = state;
    }

    public void StopPlayerInertia()
    {
        anim.SetInteger("MovementVector", 0);
        rb.velocity = Vector3.zero;
    }

    public void BurningParticle(bool status)
    {
        if(status)
        {
            fireParticle.Play();
        }
        else
        {
            fireParticle.Stop();
        }
    }
    #endregion

    #region PRIVATE_CALLS
    private void PlayerHighlightRequest()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            onHighlightRequest?.Invoke();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        RaycastHit hit;
        
        for(int i = 0; i < raycast.Count; i++)
        {
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(raycast[i].transform.position, raycast[i].transform.TransformDirection(Vector3.down), out hit, transform.localScale.y / 20.0f))
            {
                if(!canJump)
                {
                    slamDustTrail.Play();//.gameObject.SetActive(true);
                }

                canJump = true;
                break;
            }
        }
    }

    private void PlayerMovement()
    {
        if (invertMovement)
        {
            rb.velocity = new Vector3(-hor * movementSpeed, rb.velocity.y, -ver * movementSpeed);
        }
        else
        {
            rb.velocity = new Vector3(hor * movementSpeed, rb.velocity.y, ver * movementSpeed);
        }  
        anim.SetInteger("MovementVector", (int)movementDirection.magnitude);

        if (movementDirection != Vector3.zero)
        {
            if(!canJump)
                steps.Stop();
            else
            {
                if(!steps.isPlaying)
                    steps.Play();
            }

            dustTrail.gameObject.SetActive(true);
            float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref yVelocity, smoothRotation);
            rb.rotation = Quaternion.Euler(0f,angle,0f);
        }
        else
        {
            dustTrail.gameObject.SetActive(false);
        }
    }

    private void PlayerJumpLogic()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            sfxSource.pitch = 1.5f;
            sfxSource.PlayOneShot(sfxsClips[(int)SfxName.jump]);
            sfxSource.pitch = 1;

            rb.AddForce(new Vector3(0, jumpforce, 0), ForceMode.Impulse);
            canJump = false;
            //slamDustTrail.gameObject.SetActive(canJump);
            anim.SetTrigger("Jump");
        }

        if (positionY > transform.position.y)
        {
            anim.SetTrigger("Fall");
        }

        positionY = transform.position.y;
    }

    private void PlayerGrabLogic()
    {
        SpringJoint joint;
        gameObject.TryGetComponent<SpringJoint>(out joint);
        if(joint != null)
        {
            if(joint.connectedBody == null)
            {
                Destroy(joint);
            }
            else
            {
                joint.connectedBody.gameObject.TryGetComponent<DestroySetsOfComponents>(out DestroySetsOfComponents sets);
                joint.connectedBody.gameObject.TryGetComponent<MeshCollider>(out MeshCollider mesh);
                if ((mesh != null && !mesh.enabled) || (sets != null && sets.groupDestroyed))
                {
                    Destroy(joint);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (joint == null)
            {
                sfxSource.PlayOneShot(sfxsClips[(int)SfxName.atack]);
                OnGrab?.Invoke();
            }
            else
            {
                if (joint != null)
                {
                    Rigidbody grabbed = joint.connectedBody;
                    Destroy(joint);
                    Debug.Log("Throwing Grabbed Object");
                    grabbed.AddForce(new Vector3(transform.forward.x * frontForce, upForce, transform.forward.z * frontForce), ForceMode.Impulse);
                    invertMovement = false;
                }
            }
        }
    }

    private void RecieveGrabbed(Rigidbody grabbedObject)
    {
        if (!gameObject.GetComponent<SpringJoint>())
        {
            SpringJoint joint = gameObject.AddComponent<SpringJoint>();
            joint.anchor = transform.InverseTransformPoint(anchorPoint.position);
            joint.spring = springForce;
            joint.connectedMassScale = dragForce;
            joint.connectedBody = grabbedObject;
            if (grabbedObject.mass >= pushDragThreshold)
            {
                invertMovement = true;
            }
            else
            {
                invertMovement = false;
            }
        }
        Debug.Log("Recieved grabbable");
    }
    private void EnableBerserkMode()
    {
        berserkMode = true;
    }
    private void PlayerPushLogic()
    {
        if (pushCountdown >= 0)
        {
            pushCountdown -= Time.deltaTime;
        }
        else if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            sfxSource.PlayOneShot(sfxsClips[(int)SfxName.atack]);
            anim.SetTrigger("Push");
            IsPushing?.Invoke(pushTime, frontForce, upForce);
            pushCountdown = pushCooldown;
        }
    }

    private void EnterBerserkMode()
    {
        if (Input.GetKeyDown(KeyCode.Q) && berserkMode)
        {
            berserkMode = false;
            OnBerserkModeStart?.Invoke();
            StartCoroutine(BerserkMode());
        }
    }

    private IEnumerator BerserkMode()
    {
        float timer = 0;
        Color startColor = bodyRend.material.GetColor("_MainColor");
        Vector3 startScale = transform.localScale;
        Vector3 endScale = transform.localScale * 2.0f;
        jumpforce += jumpForceBuff;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            bodyRend.material.SetColor("_MainColor", Color.Lerp(bodyRend.material.GetColor("_MainColor"), tint, timer / duration));
            transform.localScale = Vector3.Lerp(transform.localScale, endScale, timer / duration);
            yield return null;
        }
        OnBerserkModeEnd?.Invoke();
        jumpforce -= jumpForceBuff;
        timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            bodyRend.material.SetColor("_MainColor", Color.Lerp(bodyRend.material.GetColor("_MainColor"), startColor, timer / duration));
            transform.localScale = Vector3.Lerp(transform.localScale, startScale, timer / duration);
            yield return null;
        }
    }

    private void BlockedMovement()
    {
        dustTrail.gameObject.SetActive(true);

        rb.velocity = new Vector3(transform.forward.x * movementSpeed, rb.velocity.y, transform.forward.z * movementSpeed);
        anim.SetInteger("MovementVector", (int)rb.velocity.magnitude);

        Quaternion rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        rb.rotation = Quaternion.RotateTowards(transform.rotation, rotation, smoothRotation);
    }
    public void BurningSFX()
    {
        sfxSource.PlayOneShot(sfxsClips[(int)SfxName.fireInjure]);
    }
    #endregion
}