using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRock : MonoBehaviour {

    public float speed = 2f;
    public float maxRotation = 45f;
    public float StartRotation = -90f;

    public GameObject Ocean;
        
    void LateUpdate()
    {
        Vector3 rotationValue;

        //+ (StartRotation + maxRotation * Mathf.Sin(Time.time * speed)) / 5
        //rotationValue = new Vector3(Camera.main.transform.rotation.eulerAngles.x , Camera.main.transform.rotation.eulerAngles.y , Camera.main.transform.rotation.eulerAngles.z);
        //Vector3 rotationValue = new Vector3(Camera.main.transform.localRotation.x + (StartRotation + maxRotation * Mathf.Sin(Time.time * speed)) / 5, Camera.main.transform.localRotation.y , Camera.main.transform.localRotation.z);

        // transform.rotation = Quaternion.Euler(rotationValue) *  Quaternion.Euler((StartRotation + maxRotation * Mathf.Sin(Time.time * speed)) / 5, 1, 1);
        rotationValue=transform.TransformDirection(Camera.main.transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, Camera.main.transform.rotation.eulerAngles.z);
        rotationValue = new Vector3(rotationValue.x + (StartRotation + maxRotation * Mathf.Sin(Time.time * speed)) / 5, rotationValue.y, rotationValue.z);
        rotationValue = transform.InverseTransformDirection(rotationValue);
        transform.rotation = Quaternion.Euler(rotationValue);


        if (Ocean != null)
        {
            
            rotationValue = new Vector3((StartRotation + maxRotation * Mathf.Sin(Time.time * speed) * -20) / 100, Ocean.transform.rotation.eulerAngles.y, Ocean.transform.rotation.eulerAngles.z);
            Ocean.transform.rotation = Quaternion.Euler(rotationValue);
        }

    }

}
