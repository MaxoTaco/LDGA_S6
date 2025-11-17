using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public float speed = 4f;
    public float gravity = 9.81f;
    public float airControl = 10f;
    public float footstepInterval = 1f;
    public AudioClip[] footstepArray;

    Vector3 input;
    Vector3 moveDirection;
    CharacterController controller;
    AudioSource audioSource;
    bool isWalking = false;

    public bool StopMovement { get; set; }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(PlayFootsteps(footstepInterval));
    }

    void Update()
    {
        if (StopMovement) return;

        // get input
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        isWalking = moveHorizontal != 0 || moveVertical != 0;

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

    IEnumerator PlayFootsteps(float interval)
    {
        while (true)
        {
            if (isWalking)
            {
                audioSource.clip = footstepArray[Random.Range(0, footstepArray.Length)];
                audioSource.Play();
                yield return new WaitForSeconds(interval);
            }
            else yield return null;
        }
    }
}