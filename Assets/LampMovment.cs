using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampMovment : MonoBehaviour
{
    public float speed = 2f;
    public float maxLoc = 45f;
    public Vector3 StartLoc;

    public HingeJoint lamp;

    private void Start()
    {
        StartLoc = transform.localPosition;
        
    }
    void LateUpdate()
    {

        if (lamp.limits.min==0)
        {
        JointLimits NewLimits=new JointLimits();
        NewLimits.min= -179.9f;
        lamp.limits = NewLimits;
        }


        Vector3 LocValue;

        LocValue = transform.localPosition;
        LocValue = new Vector3(LocValue.x , LocValue.y- (maxLoc * Mathf.Sin(Time.time * speed)) / 200, LocValue.z);
        transform.localPosition = LocValue;


    }


}
