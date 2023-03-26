using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public static Action onCaughtPlayer;

    enum states { SetDestination, Roam, Wait, Chase, Grab, Jail }
    states currentstate;

    NavMeshAgent agent;
    float roamRange = 10f;
    [SerializeField]
    float waitTime = 2f;
    [SerializeField]
    Transform head;
    [SerializeField]
    float headSwingSpeed = 2f;
    [SerializeField]
    float headSwingTime = 1f;
    [SerializeField]
    float headSwingRange = 45f;
    [SerializeField]
    Transform eyes;
    [SerializeField]
    float viewRange = 25f;
    [SerializeField]
    float viewRadius = 10f;
    [SerializeField]
    Light visionLight;
    [SerializeField]
    List<Transform> visibleObjects;
    
    Transform target;
    public Transform holdingPoint;
    public List<Transform> capturePoints;
    Transform chosenCapturePoint;
    bool capturePointChosen =false;

    public float rotDamping;
    float catchRadius = 3f;
    float depositRadius = 2f;
    float waitTimer;

    public bool playerCaught = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        currentstate = states.SetDestination;
        visibleObjects = new List<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        visionLight.range = viewRange;
        visionLight.innerSpotAngle = viewRadius*2;
        visionLight.spotAngle = viewRadius*2;

        switch (currentstate)
        {
            case states.SetDestination:
                Vector3 point;
                if (RandomPoint(transform.position, roamRange, out point))
                {
                    agent.destination = point;
                    currentstate = states.Roam;
                }
                break;
            case states.Roam:
                if (!agent.pathPending)
                {
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                        {
                            currentstate = states.Wait;
                        }
                    }
                }
                break;
            case states.Wait:
                waitTimer += Time.deltaTime;
                float finalAngle = Mathf.Sin(waitTimer*headSwingSpeed) * headSwingRange;
                head.rotation = Quaternion.Slerp(head.rotation, Quaternion.Euler(head.eulerAngles.x, finalAngle, head.eulerAngles.z), headSwingTime);
                if(waitTimer >= waitTime)
                {
                    currentstate = states.SetDestination;
                    waitTimer = 0;
                }
                break;
            case states.Chase:
                agent.destination = target.position;
                if (agent.velocity.magnitude < 0.15f)
                {
                    Vector3 lookDir = target.position - transform.position;
                    lookDir.y = 0;
                    Quaternion rot = Quaternion.LookRotation(lookDir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotDamping);
                }
                if (Vector3.Distance(target.position, transform.position) < catchRadius)
                {
                    currentstate = states.Grab;
                }
                break;
            case states.Grab:
                target.transform.position = holdingPoint.position;
                target.transform.parent = holdingPoint;
                playerCaught = true;
                for (int i = 0; i < capturePoints.Count; i++)
                {
                    if (capturePointChosen == false && capturePoints[i].childCount == 0)
                    {
                        chosenCapturePoint = capturePoints[i];
                        agent.destination = chosenCapturePoint.position;
                        capturePointChosen = true;
                    }
                }
                onCaughtPlayer?.Invoke();
                currentstate = states.Jail;
                break;
            case states.Jail:
                if (Vector3.Distance(agent.destination, transform.position) < depositRadius)
                {
                    target.transform.parent = chosenCapturePoint;
                    target.transform.position = chosenCapturePoint.transform.position;
                    currentstate = states.SetDestination;
                }
                break;
        }
        // ¿Que puedo ver en mi campo de vision?
        Collider[] targetsInRange = Physics.OverlapSphere(transform.position, viewRange);
        visibleObjects.Clear();
        for (int i = 0; i < targetsInRange.Length; i++)
        {
            Transform target = targetsInRange[i].transform;
            Vector3 dirToTarget = target.position - eyes.position;
            if (Vector3.Angle(eyes.forward, dirToTarget) < viewRadius) // ¿Esta en mi radio de vision?
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                RaycastHit hit;
                if (Physics.Raycast(eyes.position, dirToTarget, out hit, distanceToTarget)) // ¿Algo me lo obstruye?
                {
                    visibleObjects.Add(target);
                }
            }
        }
        // ¿Entre los objetos que vi, hay un player?
        if(currentstate != states.Chase && currentstate != states.Grab && currentstate != states.Jail)
        {
            for (int i = 0; i < visibleObjects.Count; i++)
            {
                if (visibleObjects[i] != null && visibleObjects[i].tag == "Player")
                {
                    target = visibleObjects[i];
                    currentstate = states.Chase;
                    return;
                }
            }
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    private void OnDrawGizmos()
    {
        Collider[] targetsInRange = Physics.OverlapSphere(transform.position, viewRange);
        visibleObjects.Clear();
        for (int i = 0; i < targetsInRange.Length; i++)
        {
            Transform target = targetsInRange[i].transform;
            Vector3 dirToTarget = target.position - eyes.position;

            if (Vector3.Angle(eyes.forward, dirToTarget) < viewRadius)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(eyes.position, dirToTarget, distanceToTarget))
                {

                    Gizmos.color = Color.red; // Dentro de cono de vision, puede verse
                    Gizmos.DrawLine(eyes.position, target.position);
                }
                else
                {
                    Gizmos.color = Color.yellow; // Dentro de cono de vision, Obstruido
                    Gizmos.DrawLine(eyes.position, target.position); 
                }
            }
            else
            {
                Gizmos.color = Color.blue; // Fuera de cono de vision
                Gizmos.DrawLine(eyes.position, target.position);
            }
        }
    }
}