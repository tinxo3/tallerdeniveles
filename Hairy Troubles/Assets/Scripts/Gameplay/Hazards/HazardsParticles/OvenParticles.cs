using UnityEngine;

public class OvenParticles : MonoBehaviour
{
    #region EXPOSED_FIELD
    [SerializeField] private ParticleSystem fireParticle = null;
    [SerializeField] private ParticleSystem gasParticle = null;
    #endregion

    #region PRIVATE_FIELD

    #endregion

    #region PUBLIC_CALLS
    public void Init()
    {
        fireParticle.Stop();
        gasParticle.Stop();

        SwapActiveParticle(true);
    }

    public void SwapActiveParticle(bool state)
    {
        if(state)
        {
            fireParticle.Stop();
            gasParticle.Play();
        }
        else
        {
            fireParticle.Play();
            gasParticle.Stop();
        }
    }
    #endregion

    #region PRIVATE_CALLS

    #endregion
}