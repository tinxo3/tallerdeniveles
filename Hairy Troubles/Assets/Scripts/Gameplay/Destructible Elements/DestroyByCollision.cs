using UnityEngine;

public class DestroyByCollision : DestructibleComponent, ICollidable
{
    public enum ObjSurface
    {
        Floor,
        Wall,
        Roof
    }

    #region EXPOSED_METHODS
    [Header("Particular Object")]
    [Range(0.01f, 20f)]
    [SerializeField] private float fractureLimit = 2.0f;
    [Space(15f)]

    [Header("Features")]
    [SerializeField] private ObjSurface objSurface = ObjSurface.Floor;
    [SerializeField] private bool destroyByPlayerCollision = false;
    #endregion

    #region UNITY_CALLS
    private void FixedUpdate()
    {
        velocity = rig.velocity.y;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Movement>() && destroyByPlayerCollision)
        {
            SwapComponent();
        }
        else
        {
            if (objSurface == ObjSurface.Floor)
            {
                if (velocity <= -fractureLimit)
                {
                    SwapComponent();
                }
            }
            else if (objSurface == ObjSurface.Wall)
            {
                if (collision.transform.GetComponent<ICollidable>() != null)
                {
                    rig.isKinematic = false;
                    objSurface = ObjSurface.Floor;
                }
            }
            else if (objSurface == ObjSurface.Roof)
            {

            }
        }
    }
    #endregion
}