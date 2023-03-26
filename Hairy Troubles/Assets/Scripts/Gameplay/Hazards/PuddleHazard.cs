using UnityEngine;

public class PuddleHazard : Hazard
{
    #region EXPOSED_FIELD
    [SerializeField] [Range(0, 180)] private float throwAngleX = 70;
    [SerializeField] private float throwForce = 130;

    [Header("Particle Splash")]
    [SerializeField] private GameObject splashPref = null;
    [SerializeField] private Vector3 splashScale = Vector3.one;
    #endregion

    #region PRIVATE_FIELD
    private const int ANGLE_DEGREES_Y = 360;
    #endregion

    #region UNITY_CALLS
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out StatusEffects player))
        {
            SFX.Play();
            int degreesY = Random.Range(0, ANGLE_DEGREES_Y);

            Vector3 direction = Quaternion.Euler(-throwAngleX, degreesY, 0) * Vector3.forward;

            GameObject go = Instantiate(splashPref, other.ClosestPoint(this.transform.position), splashPref.transform.rotation);
            go.transform.localScale = splashScale;
            Destroy(go, 3f);

            player.UpsideDownState(throwForce, direction, timerEffect);
        }
    }
    #endregion
    
    #region OVERRIDE_CALLS
    protected override void TriggerEvent()
    {

    }
    #endregion

    #region PRIVATE_CALLS

    #endregion
}