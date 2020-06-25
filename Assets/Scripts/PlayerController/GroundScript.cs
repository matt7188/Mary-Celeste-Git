using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour
{
    public FPPlayerController playerController;

    public void OnTriggerEnter(Collider other)
    {
        playerController.isGrounded = true;
        Debug.Log("Grounded");
    }

    public void OnTriggerExit(Collider other)
    {
        playerController.isGrounded = false;
        Debug.Log("Not Grounded");
    }
}
