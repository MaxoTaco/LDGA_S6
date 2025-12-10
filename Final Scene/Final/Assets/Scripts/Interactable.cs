using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections.Generic;

public enum InteractionStage {FirstInteraction, SecondInteraction, ThirdInteraction, FourthInteraction}

public class Interactable : MonoBehaviour
{
    [Header("General Settings")]
    public float detectionRange = 2f;
    public GameObject canvas;
    public GameObject player;
    public Transform cameraPosition;

    [Header("Interaction Settings")]
    public InteractionStage interactionStage;
    public float moveCameraDuration = 2f; // in seconds, time to move camera to frame the object
    public float interactionDuration = 0.25f; // in seconds, durations of the entire interaction (AFTER moveCameraDuration)
    public String[] texts;
    public Texture2D color;

    public AudioClip interactionAudio;
    public PostProcessingVolume postProcessingVolume;

    PlayerController playerController;
    MeshRenderer playerMesh;
    Transform cameraTransform;
    CameraController cameraController;
    Vector3 newCameraPosition;
    Quaternion newCameraRotation;
    AudioSource audioSource;
    bool inInteraction = false;
    UnityEvent onInteraction = new UnityEvent();
    List<SwapObject> objectsToSwap = new List<SwapObject>();

    private Animator anim;
    private TextMeshProUGUI m_TextMeshPro;

    bool done = false;

    void Start()
    {
        
        anim = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Animator>();
        m_TextMeshPro = GameObject.FindGameObjectWithTag("Canvas").GetComponentInChildren<TextMeshProUGUI>();

        if (!player) player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        playerMesh = player.GetComponent<MeshRenderer>();
        cameraTransform = Camera.main.transform;
        cameraController = cameraTransform.GetComponent<CameraController>();

        if (!cameraPosition) cameraPosition = transform.Find("Camera Position");

        newCameraPosition = cameraPosition.position;
        newCameraRotation = cameraPosition.rotation;
        
        audioSource = cameraTransform.GetComponent<AudioSource>();

        var swapObjects = FindObjectsByType<SwapObject>(FindObjectsSortMode.None);
        foreach (var swapObject in swapObjects)
        {
            if (swapObject && swapObject.interactionStage == interactionStage) objectsToSwap.Add(swapObject);
        }
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
        playerMesh.enabled = false;

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
        anim.SetTrigger("Close");
        // play audio
        if (interactionAudio) audioSource.PlayOneShot(interactionAudio);

        // post processing effects here
        // ^ seeing what it looks like if I apply here. Fade out should take some time to run.
        if (postProcessingVolume) postProcessingVolume.GetComponent<PostProcessingVolume>().FadeOut(interactionDuration);

        
        foreach(String s in texts)
        {
            for (int i = 0; i < s.Length; i++)
            {
                m_TextMeshPro.text += s[i];
                yield return new WaitForSeconds(0.05f);
            }


            yield return new WaitForSeconds(1f);

            while (m_TextMeshPro.text.Length > 0)
            {
                m_TextMeshPro.text = m_TextMeshPro.text.Substring(0, m_TextMeshPro.text.Length - 1);
                yield return new WaitForSeconds(0.02f);
            }
        }
        


        // change environment here
        onInteraction.Invoke();
        foreach(SwapObject i in objectsToSwap)
        {
            i.Swap();
        }
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Leaf"))
        {
            o.GetComponent<MeshRenderer>().materials[1].SetTexture("_BaseMap", color);
        }
            


        // end blink here, open eyes
        // wait for some seconds

        anim.SetTrigger("Open");
        

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

        if (interactionStage == InteractionStage.ThirdInteraction)
        {
            Debug.Log("attempting to animate");
            FindFirstObjectByType<AnimateObject>().Animate();
        }

        // snap to at the end
        cameraTransform.position = originalCameraPostion;
        cameraTransform.rotation = originalCameraRotation;

        playerController.StopMovement = false;
        cameraController.StopMovement = false;
        playerMesh.enabled = true;

        inInteraction = false;
        enabled = false;

        
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