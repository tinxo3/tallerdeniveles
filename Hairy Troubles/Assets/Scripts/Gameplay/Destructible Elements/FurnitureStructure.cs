using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureStructure : DestructibleComponent
{
    [Header("Destroy By Collision")]
    public bool destroyByCollision = false;
    [Range(0.01f, 20f)]
    [SerializeField] private float fractureLimit = 1.0f;

    public List<GameObject> furnitureComponents;

    private FixedJoint fixedJoint;

    // -----------------

    protected override void Awake()
    {
        base.Awake();

        AwakeList();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (destroyByCollision)
        {
            if (rig.mass < collision.rigidbody.mass && rig.velocity.magnitude >= fractureLimit)
            {
                if (fixedJoint != null) Destroy(fixedJoint);
                SwapComponent();
                DisarmFurniture();
            }
        }
    }
    
    private void AwakeList()
    {
        foreach (GameObject obj in furnitureComponents)
        {
            if (obj.GetComponent<FixedJoint>() == null)
            {
                obj.AddComponent<FixedJoint>();
                obj.GetComponent<FixedJoint>().connectedBody = this.rig;
            }
            else if(!obj.GetComponent<FixedJoint>().connectedBody)
            {
                obj.GetComponent<FixedJoint>().connectedBody = this.rig;
            }

            obj.GetComponent<MeshRenderer>().enabled = false;
            obj.GetComponent<MeshCollider>().isTrigger = true;
        }
    }

    private void DisarmFurniture()
    {
        foreach(GameObject obj in furnitureComponents)
        {
            Destroy(obj.GetComponent<FixedJoint>());

            obj.GetComponent<MeshRenderer>().enabled = true;
            obj.GetComponent<MeshCollider>().isTrigger = false;
        }
    }

}
