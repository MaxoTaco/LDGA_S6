using UnityEngine;

public class AdjustFov : MonoBehaviour
{
    public Camera playerCamera;
    public float normalFOV = 60f;
    public float zoomFOV = 40f;
    public float zoomSpeed = 10f;

    private bool isZooming = false;

    void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    void Update()
    {
        // Check if middle mouse button is pressed
        if (Input.GetMouseButtonDown(1))
            isZooming = true;
        if (Input.GetMouseButtonUp(1))
            isZooming = false;

        // Smoothly interpolate the camera’s FOV
        float targetFOV = isZooming ? zoomFOV : normalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }
}