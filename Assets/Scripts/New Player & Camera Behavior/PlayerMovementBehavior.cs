using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementBehavior : MonoBehaviour
{
    public Transform playerModel;

    [Header("Movement")]
    public float moveSpeed;
    public float walkSpeed = 6f;
    public float sneakSpeed = 3f;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    bool readyToJump;

    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sneakKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Crouch Settings")]
    public float standingHeight = 2f;
    public float crouchHeight = 1f;

    public Transform playerCamera;
    public float standingCamY = 1.6f;
    public float crouchCamY = 1.0f;
    public float crouchSpeed = 10f;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    bool isSneaking;

    Vector3 moveDirection;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
        moveSpeed = walkSpeed;
    }

    private void Update()
    {
        grounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            playerHeight * 0.5f + 0.3f,
            whatIsGround
        );

        MyInput();
        HandleSneak();
        SpeedControl();

        rb.drag = grounded ? groundDrag : 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        isSneaking = Input.GetKey(sneakKey);
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput +
                        orientation.right * horizontalInput;

        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void HandleSneak()
    {
        moveSpeed = isSneaking ? sneakSpeed : walkSpeed;
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}