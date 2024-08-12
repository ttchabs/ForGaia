using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Orientation")]
    public Transform orientation;

    #region PUBLIC FIELDS:
    [Header("Movement")]
    public float moveSpeed;
    public float gravity = -9.8f;

    [Header("Jump")]
    public float JumpHeight;
    public LayerMask grounMasks;

    #endregion

    #region PRIVATE FIELDS:
    float horizInput;
    float vertInput;
    Vector3 moveDirect;
    #endregion

    #region INITIALIZATIONS:
    private Rigidbody rb;
    //private CharacterController characterController;
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        //characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        KeyInputs(); 
        JumpPlayer();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    public void KeyInputs()
    {
        horizInput = Input.GetAxisRaw("Horizontal");
        vertInput = Input.GetAxisRaw("Vertical");
    }

    public void MovePlayer()
    {
        moveDirect = orientation.forward * vertInput + orientation.right * horizInput;
        //characterController.Move(moveDirect * moveSpeed * Time.deltaTime);
        rb.AddForce(moveDirect.normalized * moveSpeed * Time.deltaTime * 10f, ForceMode.Force);
    }

    public void JumpPlayer()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * JumpHeight, ForceMode.Impulse);
        }
    }
}
