using System;
using System.Collections;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("General Settings")]
    public float detectionRange = 2f;
    public GameObject canvas;
    public GameObject player;

    [Header("Interaction Settings")]
    public float moveCameraDuration = 2f; // in seconds, time to move camera to frame the object
    public float interactionDuration = 10f; // in seconds, durations of the entire interaction (AFTER moveCameraDuration)
    public float distanceFromCamera = .5f; // how far the camera will be from the object
    public AudioClip interactionAudio;

    PlayerController playerController;
    Transform cameraTransform;
    CameraController cameraController;
    Vector3 newCameraPosition;
    Quaternion newCameraRotation;
    AudioSource audioSource;
    bool inInteraction = false;

    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        cameraTransform = Camera.main.transform;
        cameraController = cameraTransform.GetComponent<CameraController>();

        // can be adjusted later for different postions (vertical, different orientations)
        newCameraPosition = transform.position - Vector3.forward * distanceFromCamera;
        newCameraRotation = transform.rotation;
        
        audioSource = cameraTransform.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (inInteraction) return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);

        bool inRange = false;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                inRange = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(StartInteraction());
                    return;
                }
            }
        }
        canvas.SetActive(inRange);
    }

    IEnumerator StartInteraction()
    {
        inInteraction = true;
        canvas.SetActive(false);

        // freezes player and camera movement
        playerController.StopMovement = true;
        cameraController.StopMovement = true;

        // save initial camera position
        Vector3 originalCameraPostion = cameraController.transform.position;
        Quaternion originalCameraRotation = cameraController.transform.rotation;

        // lerp to new position
        // may want to gradually apply post processing in here as well
        float elapsedTime = 0f;

        while (elapsedTime < moveCameraDuration)
        {
            cameraTransform.position = Vector3.Lerp(originalCameraPostion,
                                                    newCameraPosition,
                                                    EaseInOut(elapsedTime / moveCameraDuration));
            cameraTransform.rotation = Quaternion.Lerp(originalCameraRotation,
                                                       newCameraRotation,
                                                       EaseInOut(elapsedTime / moveCameraDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // snap to at the end
        cameraTransform.position = newCameraPosition;
        cameraTransform.rotation = newCameraRotation;

        // play audio
        if (interactionAudio) audioSource.PlayOneShot(interactionAudio);

        // post processing effects here

        yield return new WaitForSeconds(interactionDuration);

        // start blink here, close eyes (goes to black screen)

        // change environment here

        // end blink here, open eyes

        // move camera back
        elapsedTime = 0;

        while (elapsedTime < moveCameraDuration)
        {
            cameraTransform.position = Vector3.Lerp(newCameraPosition,
                                                    originalCameraPostion,
                                                    EaseInOut(elapsedTime / moveCameraDuration));
            cameraTransform.rotation = Quaternion.Lerp(newCameraRotation,
                                                       originalCameraRotation,
                                                       EaseInOut(elapsedTime / moveCameraDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // snap to at the end
        cameraTransform.position = originalCameraPostion;
        cameraTransform.rotation = originalCameraRotation;

        playerController.StopMovement = false;
        cameraController.StopMovement = false;

        inInteraction = false;
    }

    // formula from https://easings.net/#easeInOutSine
    float EaseInOut(float t)
    {
        return -(Mathf.Cos((float)(Math.PI * t)) - 1) / 2;
    }

    // visualize detection range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}