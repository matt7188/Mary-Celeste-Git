using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shadowPeopleBehavior : MonoBehaviour
{
    
        Vector3 NewPos;
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position, NewPos) < .2f)
        {

            

                NewPos = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y, transform.position.z + Random.Range(-1f, 1f));

            if (Vector3.Distance(this.transform.parent.position, NewPos) > 5)
                NewPos = this.transform.parent.position;
        }



            this.transform.position = Vector3.Lerp(this.transform.position,NewPos,5*Time.deltaTime);
        this.transform.Rotate(0, 5, 0);

            }
}
