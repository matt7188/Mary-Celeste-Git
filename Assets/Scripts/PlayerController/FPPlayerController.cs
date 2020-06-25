using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPPlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 2.0f;

    Vector3 moveDirection;

    public Vector3 GravityVector;

    [SerializeField]
    private float gravityMagnitude = 10f;

    [SerializeField]
    private float gravityRotationModifier = 2.0f;

    private Rigidbody rigidBody;

    private bool canMove = true;

    public bool isGrounded = true;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //if(Input.GetKey(KeyCode.W))
        //{
        //    transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        //}

        //if (Input.GetKey(KeyCode.S))
        //{
        //    transform.Translate(Vector3.forward * -moveSpeed * Time.deltaTime);
        //}

        //if (Input.GetKey(KeyCode.D))
        //{
        //    transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        //}

        //if (Input.GetKey(KeyCode.A))
        //{
        //    transform.Translate(Vector3.right * -moveSpeed * Time.deltaTime);
        //}

        if(isGrounded)
        {
            canMove = true;
        }

        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = (horizontalMovement * transform.right + verticalMovement * transform.forward).normalized;
        
        if(Input.GetKeyDown(KeyCode.J))
        {
            ModifyGravity(new Vector3(-1, 0, 0));
            canMove = false;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            ModifyGravity(new Vector3(1, 0, 0));
            canMove = false;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            ModifyGravity(new Vector3(0, 0, 1));
            canMove = false;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            ModifyGravity(new Vector3(0, 0, -1));
            canMove = false;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            ModifyGravity(new Vector3(0, 1, 0));
            canMove = false;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            ModifyGravity(new Vector3(0, -1, 0));
            canMove = false;
        }

        GravityEffect();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (canMove)
        {
            Vector3 verticalVelocity = Vector3.Dot(GravityVector, rigidBody.velocity) * GravityVector / (GravityVector.magnitude * GravityVector.magnitude);
            rigidBody.velocity = moveDirection * moveSpeed * Time.deltaTime;
            rigidBody.velocity += verticalVelocity;
        }
    }

    private void ModifyGravity(Vector3 newGravity)
    {
        GravityVector = newGravity;
    }

    private void GravityEffect()
    {
        rigidBody.AddForce(GravityVector * gravityMagnitude, ForceMode.Acceleration);

        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -GravityVector) * transform.rotation;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, gravityRotationModifier * Time.deltaTime);
    }
}
