using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class RatController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed;

    public float jumpHeight;
    public float gravity;
    public float airControl;
    public float rotationSpeed;
    
    private CharacterController cc;
    private Animator anim;
    private Vector3 moveDirection = Vector3.zero;

    public GameObject armature;
    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 input = GetInput();
        HandleMovement(input);
        RotateRat(input);

    }

    private void RotateRat(Vector3 input)
    {
        if (input.sqrMagnitude > 0.01f)
        {
            armature.transform.rotation = Quaternion.Slerp(armature.transform.rotation, Quaternion.LookRotation(input), Time.deltaTime * rotationSpeed);
        }

    }

    private Vector3 GetInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        return (transform.right * x + transform.forward * z).normalized;
    }

    private void HandleMovement(Vector3 input)
    {
        //if we are on the ground check for jump, and set movement
        if (cc.isGrounded)
        {   
            // Zero out horizontal movement if no input
            if (input.sqrMagnitude > 0.01f)
            {
                anim.SetBool("isRunning", true);
                moveDirection = new Vector3(input.x, moveDirection.y, input.z);
            }
            else
            {
                anim.SetBool("isRunning", false);
                moveDirection = new Vector3(0, moveDirection.y, 0);
            }

            moveDirection.y = Input.GetButton("Jump") ? Mathf.Sqrt(2 * jumpHeight * gravity) : 0f;
                
        }
        //if we are not on the ground then adjust our movement based on air control
        else
        {
            moveDirection = Vector3.Lerp(moveDirection, new Vector3(input.x, moveDirection.y, input.z), airControl * Time.deltaTime);
        }
        
        moveDirection.y -= gravity * Time.deltaTime;
        cc.Move(moveDirection * speed * Time.deltaTime);
    }
}
