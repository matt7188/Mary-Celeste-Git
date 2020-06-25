using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafePuzzle : MonoBehaviour
{
    private const float startAngle = 45;
    //Quaternion originalRotation;
    public bool reverse;
    public bool Active;

    public float FinalNumber;
    static public float MarginOfError=10;
    public bool Correct;

    Vector3 lastPos;
    Vector3 delta;

    public float SendTolargeSafe;

    void Start()
    {
       // originalRotation = this.transform.rotation;
        Correct = false;
    }
    void Update()
    {

 if (Input.GetMouseButtonDown(0))
            {
                lastPos = Input.mousePosition;
            }

        if (Input.GetMouseButton(0) && Active)
        {

            /*  Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);

              Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
              Vector3 vector = Input.mousePosition - screenPos;
              float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg * 5;
              Quaternion newRotation = Quaternion.AngleAxis(angle - startAngle, this.transform.forward);
              newRotation.y = 0; //see comment from above 
              newRotation.eulerAngles = new Vector3(0, 0, newRotation.eulerAngles.z);
              if (reverse)
                  newRotation.eulerAngles *= -1;
              this.transform.rotation = originalRotation * newRotation;
              //Debug.Log(originalRotation * newRotation);*/

            delta = Input.mousePosition - lastPos;


            lastPos = Input.mousePosition;

            float adX = -delta.x / 5;
            float adY = delta.y / 5;

            if (Input.mousePosition.x < this.transform.position.x)
                adY *= -1;
            if (Input.mousePosition.y < this.transform.position.y)
                adX *= -1;

            this.transform.Rotate(0, 0, adX + adY);
            SendTolargeSafe = adX + adY;

        }
        else
        {
            //originalRotation = this.transform.rotation;
            SendTolargeSafe = 0;
        }


        //Debug.Log(transform.eulerAngles.z);

        float NumToDeg = (FinalNumber * 360) / 100;

        if (transform.eulerAngles.z + MarginOfError > NumToDeg && transform.eulerAngles.z - MarginOfError < NumToDeg)
        {
            Correct = true;
        }
        else
            Correct = false;
    }
}
