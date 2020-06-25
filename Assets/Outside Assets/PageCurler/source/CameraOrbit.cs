using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour 
{
    public float speed = 100f;

	void Update () 
    {
        float angleDeltaH = 0f;
        if (Input.GetKey(KeyCode.LeftArrow))
            angleDeltaH -= 1f;
        if (Input.GetKey(KeyCode.RightArrow))
            angleDeltaH += 1f;

        Vector3 pos = transform.position;
        pos = Quaternion.AngleAxis(angleDeltaH * speed * Time.deltaTime, Vector3.up) * pos;
        transform.position = pos;
        transform.LookAt(Vector3.zero);
	}
}
