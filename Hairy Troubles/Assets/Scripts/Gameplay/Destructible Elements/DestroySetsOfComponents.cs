using UnityEngine;

public class DestroySetsOfComponents : MonoBehaviour
{
    public enum ObjSurface
    {
        Floor,
        Wall,
        Roof
    }

    #region PUBLIC_METHODS
    [SerializeField] AudioSource SFXaudio;
    [SerializeField] private DestructibleComponent[] destructibleComponents;
    [SerializeField] private bool destroyByPlayerCollision = true;
    [Range(0.01f, 20f)]
    [SerializeField] private float fractureLimit = 2.0f;
    public bool groupDestroyed =false;
    #endregion

    #region PRIVATE_METHODS
    private Rigidbody rig;
    private Collider meshCollider;
    private float velocity = 0f;
    #endregion

    #region UNITY_CALLS
    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        meshCollider = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        velocity = rig.velocity.y;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Movement>() && destroyByPlayerCollision)
        {
            foreach (DestructibleComponent item in destructibleComponents)
            {
                if (SFXaudio != null)
                {
                    SFXaudio.Play();
                }
                item.SwapComponent();
            }
            groupDestroyed = true;
        }
        else
        {
            if (velocity <= -fractureLimit)
            {
                foreach (DestructibleComponent item in destructibleComponents)
                {
                    if (SFXaudio != null)
                    {
                        SFXaudio.Play();
                    }
                    item.SwapComponent();
                }
                groupDestroyed = true;
            }
        }
    }
    #endregion
}