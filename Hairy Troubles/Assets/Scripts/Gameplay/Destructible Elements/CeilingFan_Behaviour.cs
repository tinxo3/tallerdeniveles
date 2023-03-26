using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingFan_Behaviour : MonoBehaviour
{
    [Header("Connector")]
    [SerializeField] private Transform connector;
    [SerializeField] private GameObject electricParticle;
    [SerializeField] private float upForce = 10f;

    [Header("Fan Animation")]
    [SerializeField] private GameObject fanObj;
    [SerializeField] private Vector3 rotation;
    [Range(0.1f, 20)]
    [SerializeField] private float speed = 2f;

    private FixedJoint fixedJoint;
    private bool collapse = false;

    // -----------------------

    private void Awake()
    {
        fixedJoint = GetComponent<FixedJoint>();
    }

    private void Update()
    {
        InputCollapse();
        MoveFan();
    }

    private void InputCollapse()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !collapse)
        {
            collapse = true;

            var go = Instantiate(electricParticle, connector.position, Quaternion.Euler(electricParticle.transform.rotation.eulerAngles));
            go.transform.parent = connector.transform;

            go.transform.localScale = electricParticle.transform.localScale;

            Vector3 vect = new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f)) * (upForce * 10);
            connector.GetComponent<Rigidbody>().AddForce(vect, ForceMode.Acceleration);

            Destroy(fixedJoint);
        }
    }

    private void MoveFan()
    {
        if(!collapse)
            fanObj.transform.Rotate(rotation * speed * Time.deltaTime);
    }

}
