using UnityEngine;

public class RotatePrompt : MonoBehaviour
{
    Transform playerCamera;

    void OnEnable()
    {
        playerCamera = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(playerCamera);
        transform.rotation = Quaternion.LookRotation(playerCamera.forward);
    }
}
