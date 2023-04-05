using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float movementSpeed = 6f;
    [SerializeField] float jumpForce = 5f;
    float horizontalInput;
    float verticalInput;

    [SerializeField] Animator playerAnimator;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;

    new Camera camera;
    Animator cameraAnimator;
    [SerializeField] GameObject crosshairCanvas;
    [SerializeField] GameObject Pistol;

    private void Start()
    {
        camera = Camera.main;
        cameraAnimator = camera.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        Vector3 moveVector = new Vector3(horizontalInput, 0, verticalInput);
        //rb.velocity = new Vector3(horizontalInput * movementSpeed, rb.velocity.y, verticalInput * movementSpeed);
        transform.Translate(moveVector * movementSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            playerAnimator.SetTrigger("Jump");
        }


        HandleMoveAnim();
        HandleWeaponSwitch();
    }
    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.1f, ground);
    }
    void HandleMoveAnim()
    {
        if (verticalInput > 0) playerAnimator.SetBool("RunForward", true);
        else playerAnimator.SetBool("RunForward", false);
        if (verticalInput < 0) playerAnimator.SetBool("RunBackward", true);
        else playerAnimator.SetBool("RunBackward", false);
        if (horizontalInput < 0) playerAnimator.SetBool("Left", true);
        else playerAnimator.SetBool("Left", false);
        if (horizontalInput > 0) playerAnimator.SetBool("Right", true);
        else playerAnimator.SetBool("Right", false);

        //Debug.Log(rb.velocity.y);
        if (rb.velocity.y < -10 && !IsGrounded()) playerAnimator.SetBool("Fall", true);
        else playerAnimator.SetBool("Fall", false);
    }
    void HandleWeaponSwitch()
    {
        if (Shooting.crntWeapon == 0)
        {
            cameraAnimator.SetBool("WeaponCam", false);
            crosshairCanvas.SetActive(false);
            playerAnimator.SetBool("Pistol", false);
            Pistol.SetActive(false);
        }
        else
        {
            cameraAnimator.SetBool("WeaponCam", true);
            crosshairCanvas.SetActive(true);
            playerAnimator.SetBool("Pistol", true);
            Pistol.SetActive(true);

            if (Input.GetButton("Fire2")) cameraAnimator.SetBool("AimCam", true);
            else cameraAnimator.SetBool("AimCam", false);
        }
        //Debug.Log(playerAnimator.GetBool("Pistol"));
    }
}
