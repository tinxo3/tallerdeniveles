using UnityEngine;

public class FurnitureParts : DestructibleComponent
{
    [Header("Furniture Part")]
    public bool isDestoyed = false;
    [Range(1, 20)]
    //[SerializeField] private float timeToDestroy = 5f;

    public FixedJoint fixedJoint;

    // -------------------------------

    protected override void Awake()
    {
        base.Awake();

        fixedJoint = GetComponent<FixedJoint>();
    }

    void FixedUpdate()
    {
        velocity = rig.velocity.magnitude;
    }

    public void CollapseComponent()
    {

    }

    // -------------------------------

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent<PushCollider>(out PushCollider collider);

        if(collider !=null && collider.pushing)
        {
            if (fixedJoint != null) Destroy(fixedJoint);
            SwapComponent();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.transform.tag == "Player")
        //{
        //    if (fixedJoint != null) Destroy(fixedJoint);
        //    SwapComponent();
        //}
    }
}