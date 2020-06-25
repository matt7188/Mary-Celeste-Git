using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMove : MonoBehaviour {

    public static bool Lock;
    public bool MoveOnStart;
    public GameObject CameraPivot;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    public float minimumX = -60F;
    public float maximumX = 60F;
    public float minimumY = -360F;
    public float maximumY = 360F;
    public float rotationX = 0F;
    public float rotationY = 0F;
    Quaternion originalRotation;

    public FPPlayerController playerController;

    void Update()
    {
        if (!Lock)
        {

            if (axes == RotationAxes.MouseXAndY)
            {
                rotationY /*+*/= Input.GetAxis("Mouse X") * sensitivityY;
                rotationX += Input.GetAxis("Mouse Y") * sensitivityX;

                rotationX = Mathf.Clamp(rotationX, minimumX, maximumX);

                transform.Rotate(new Vector3(0, rotationY, 0));

                CameraPivot.transform.localEulerAngles = new Vector3(-rotationX, 0, 0);

                //// Read the mouse input axis
                //rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                //rotationX += Input.GetAxis("Mouse X") * sensitivityX;
                //rotationX = ClampAngle(rotationX, minimumX, maximumX);
                //rotationY = ClampAngle(rotationY, minimumY, maximumY);
                //Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, transform.up);
                //Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
                ////CameraPivot.transform.localRotation = originalRotation * xQuaternion * yQuaternion;

                //CameraPivot.transform.localRotation = yQuaternion;

                ////transform.localRotation = xQuaternion;

                ////transform.rotation = originalRotation * xQuaternion;
            }
            //else if (axes == RotationAxes.MouseX)
            //{
            //    rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            //    rotationX = ClampAngle(rotationX, minimumX, maximumX);
            //    Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            //    CameraPivot.transform.localRotation = originalRotation * xQuaternion;
            //}
            //else
            //{
            //    rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            //    rotationY = ClampAngle(rotationY, minimumY, maximumY);
            //    Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
            //    CameraPivot.transform.localRotation = originalRotation * yQuaternion;
            //}
        }
    }
    void Start()
    {
        Lock = false;
        //GetComponent<ARFC.FPController>().enabled = MoveOnStart;
        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
        originalRotation = transform.localRotation;
        // GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
    }
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}