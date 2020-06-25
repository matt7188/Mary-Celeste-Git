using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatMove : MonoBehaviour {

    Animator animator;
    Animation currentAnimation;
    public AnimationClip TestingLength;
    public float speed=.5f;
    public float distance = 3f;
    Vector3 direction;
    Vector3 start;
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        start = transform.position;
        StartCoroutine(RatMoveAction());
	}
    void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation( transform.position-direction);
    }
    IEnumerator RatMoveAction()
    {

        direction = new Vector3(start.x + Random.Range(-distance, distance), start.y, start.z +Random.Range(-distance, distance));
       
        animator.SetBool("moving", true);
        while (true)
        {
               yield return new WaitForSeconds(.01f);
            if (Vector3.Distance(transform.position, direction) > .3)
            {
                //newDir = Vector3.RotateTowards(transform.forward, direction, step, 0.0f);
                transform.rotation = Quaternion.LookRotation(direction);
                //animator.rootRotation = Quaternion.LookRotation(direction);
                transform.position = Vector3.Lerp(transform.position, direction, Time.deltaTime * speed);
            }
            else
            {
                animator.SetBool("moving", false);
                while (animator.GetCurrentAnimatorStateInfo(0).IsName("rat_walk"))
                    yield return new WaitForSeconds(.1f);
                while (animator.GetCurrentAnimatorStateInfo(0).IsName("rat_idle_2"))
                    yield return new WaitForSeconds(.1f);

                direction = new Vector3(start.x + Random.Range(-distance, distance), start.y, start.z + Random.Range(-distance, distance));
                //animator.rootRotation = Quaternion.LookRotation(direction);
                animator.SetBool("moving", true);
                yield return new WaitForSeconds(.1f);
                //while (transform.rotation)

            }

        }
    }
}
