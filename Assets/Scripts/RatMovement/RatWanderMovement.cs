using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RatWanderMovement : MonoBehaviour
{

    NavMeshAgent navMeshAgent;

    public float distanceToNextPosition;

    Animator animator;

    private bool canMove = true;

    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();

        animator = this.GetComponentInChildren<Animator>();

        if (navMeshAgent == null)
        {
            Debug.LogError("Nav Mesh agent component is not attached to " + gameObject.name);
        }
        else
        {
            SetDestination();
        }
    }

    void Update()
    {
        if (!navMeshAgent.pathPending && !canMove)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    canMove = true;
                    animator.SetBool("walking", false);
                    Invoke("SetDestination", 2.5f);
                }
            }
        }

    }

    public void SetDestination()
    {

        Vector3 destination = RandomNavPoint(gameObject.transform.position, distanceToNextPosition, -1);

        navMeshAgent.SetDestination(destination);

        animator.SetBool("walking", true);


        canMove = false;
    }

    public Vector3 RandomNavPoint(Vector3 origin, float distance, int layerMask)
    {
        //Wandering AI
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layerMask);

        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = navHit.position;

        return navHit.position;
    }
}
