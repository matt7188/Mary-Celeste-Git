using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swinging_Lamp : MonoBehaviour {
    public float speed = 2f;
    public float maxRotation = 45f;
    public float StartRotation = -90f;

    public bool LeftToRight = true;

    private Vector3 StartingRotation;

    private void Start()
    {
        StartingRotation = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
       
    }

    void Update ()
    {
        //transform.Rotate(Mathf.Sin(Time.time * speed+(Mathf.PI/6)),0,0);
        
     
        if (LeftToRight)
            transform.rotation = Quaternion.Euler(StartRotation + maxRotation * Mathf.Sin(Time.time * speed), StartingRotation.y, StartingRotation.z);
        else
            transform.rotation = Quaternion.Euler(StartingRotation.x,StartRotation + maxRotation * Mathf.Sin(Time.time * speed), StartingRotation.z);
    
    }

}
