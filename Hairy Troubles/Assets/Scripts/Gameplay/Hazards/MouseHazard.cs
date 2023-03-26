using UnityEngine;

public class MouseHazard : Hazard
{
    #region EXPOSED_FIELD
    [Header("Particle Stars")]
    [SerializeField] private GameObject starsPref = null;
    #endregion

    #region PRIVATE_FIELD
    private Animator animator = null;
    #endregion

    #region UNITY_CALLS
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out StatusEffects player))
        {
            SFX.Play();
            player.TrappedState(timerEffect);

            EnableCollider(false);

            Vector3 newPosition = other.ClosestPoint(new Vector3(this.transform.position.x, other.transform.position.y, this.transform.position.z));
            GameObject go = Instantiate(starsPref, newPosition, starsPref.transform.rotation);
            Destroy(go, 3f);

            animator.SetBool("Activate", true);
        }
    }
    #endregion

    #region OVERRIDE_CALLS
    protected override void TriggerEvent()
    {

    }
    #endregion
}