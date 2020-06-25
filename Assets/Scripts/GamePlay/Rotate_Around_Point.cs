using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_Around_Point : MonoBehaviour {
    float speed   = 550.0f; //how fast the object should rotate
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speed);
    }
}
