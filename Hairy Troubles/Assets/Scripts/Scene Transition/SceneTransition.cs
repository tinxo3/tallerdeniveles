using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using System;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private bool awakeWithAnimation = false;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        if(awakeWithAnimation)
        {
            anim.SetTrigger("FirstAwake");
        }
    }

    public Animator GetAnimator()
    {
        return anim;
    }

    public void ChangeAnimation(float secondsDelay, Action function = null)
    {
        IEnumerator ChangeScene()
        {
            if (!awakeWithAnimation)
            {
                anim.SetTrigger("FirstChange");
            }
            else
            {
                anim.SetTrigger("Change");
            }

            yield return new WaitForSeconds(secondsDelay);

            function?.Invoke();
        }

        StartCoroutine(ChangeScene());
    }
}