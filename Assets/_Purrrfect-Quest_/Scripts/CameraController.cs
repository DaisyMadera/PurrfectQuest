using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float normalFOV = 38f;     // The default field of view
    public float zoomedFOV = 11f;     // The field of view when zoomed in
    public float zoomSpeed = 5f;      // The speed at which the camera zooms
    private float targetFOV;          // The target field of view
    private Camera cam;               // Reference to the Camera component

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.fieldOfView = normalFOV;
        targetFOV = normalFOV;
    }

    void Update()
    {
        // Follow the player's position
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        // Smoothly interpolate the camera's FOV towards the target FOV
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
    }

    // Method to call when zooming in
    public void ZoomIn()
    {
        targetFOV = zoomedFOV;
    }

    // Method to call when zooming out
    public void ZoomOut()
    {
        targetFOV = normalFOV;
    }
}


