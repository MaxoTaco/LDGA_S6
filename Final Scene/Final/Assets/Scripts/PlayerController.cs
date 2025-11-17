using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 4f;
    public float gravity = 9.81f;
    public float airControl = 10f;

    Vector3 input;
    Vector3 moveDirection;
    CharacterController controller;
    AudioSource audioSource;

    public bool StopMovement { get; set; }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    void Update()
    {
        if (StopMovement) return;

        // get input
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        if (audioSource) audioSource.mute = moveHorizontal == 0 && moveVertical == 0;

        // input vector
        input = transform.right * moveHorizontal + transform.forward * moveVertical;
        input.Normalize();

        if (controller.isGrounded)
        {
            moveDirection = input;
            moveDirection.y = 0.0f;
        }
        else
        {
            // midair
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * speed * Time.deltaTime);
    }
}