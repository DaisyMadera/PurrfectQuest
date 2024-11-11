using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayTrigger : MonoBehaviour
{
    private CameraController cameraController;

    void Start()
    {
        // Find the CameraController in the scene
        cameraController = FindObjectOfType<CameraController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cameraController.ZoomIn();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cameraController.ZoomOut();
        }
    }
}


