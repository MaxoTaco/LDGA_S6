using UnityEngine;

// taken from Game Programming class, slightly modified
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float jumpHeight = 0.4f;
    public float gravity = 9.81f;
    public float airControl = 10f;

    Vector3 input;
    Vector3 moveDirection;
    CharacterController controller;
    AudioSource audioSource;
    bool hasLanded = false;
    private bool causingFootsteps = false;

    void Start()
    {
        
        controller = GetComponent<CharacterController>();
        audioSource = GetComponentInChildren<AudioSource>();
        audioSource.Play();
    }

    void Update()
    {
        // get input
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // input vector
        input = transform.right * moveHorizontal + transform.forward * moveVertical;
        input.Normalize();


        moveDirection = input;

        PerformJumpLogic();

        if (moveDirection.x != 0 || moveDirection.z != 0)
        {
            causingFootsteps = true;
        }
        else
        {
            {
                causingFootsteps = false;
            }
        }

        if (audioSource.isPlaying && causingFootsteps == false)
        {
            audioSource.Pause();
        }
        
        if (!audioSource.isPlaying && causingFootsteps == true)
        {
            audioSource.UnPause();
        }


    controller.Move(moveDirection * speed * Time.deltaTime);
    }

    private void PerformJumpLogic()
    {
        if (controller.isGrounded)
        {
            // landing particle effects
            if (!hasLanded)
            {
                hasLanded = true;
            }

            // jump

            if (Input.GetButton("Jump"))
            {
                /*
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
                hasLanded = false;
                */
            }
            else
            {
                moveDirection.y = 0.0f;
            }
        }
        else
        {
            // midair
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
        }

        moveDirection.y -= gravity * Time.deltaTime;
    }
    
    
}