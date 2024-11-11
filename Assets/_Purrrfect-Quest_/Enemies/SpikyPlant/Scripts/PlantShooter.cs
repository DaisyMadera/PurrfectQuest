using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantShooter : MonoBehaviour
{
    public GameObject spikePrefab;
    public float shootInterval= 1f;
    public float spikeSpeed = 5f;

    private Vector2 direction;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShootSpikes());

    }

    IEnumerator ShootSpikes()
    {
         while (true) // This will keep shooting indefinitely
        {
            // Get the plant's position in screen space (using WorldToScreenPoint)
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            
            if (screenPosition.x < Screen.width / 2 && screenPosition.y > Screen.height / 2) // Top Left
            {
                direction = Vector2.down;
                ShootSpikeInDirection(direction, 0f);
            }
            else if (screenPosition.x > Screen.width / 2 && screenPosition.y > Screen.height / 2) // Top Right
            {
                direction = Vector2.down;
                ShootSpikeInDirection(direction, 0f);
            }
            else if (screenPosition.y < Screen.height / 2 && screenPosition.x < Screen.width / 2) // Bottom Left
            {
                direction = Vector2.up;
                ShootSpikeInDirection(direction, 180f);
            }
            else if (screenPosition.y < Screen.height / 2 && screenPosition.x > Screen.width / 2) // Bottom Right
            {
                direction = Vector2.up;
                ShootSpikeInDirection(direction, 180f);
            }
            else if (screenPosition.x > Screen.width / 2) // Right side of the screen
            {
                direction = Vector2.left;
                ShootSpikeInDirection(direction, -90f);
            }
            else if (screenPosition.x < Screen.width / 2) // Left side of the screen
            {
                direction = Vector2.right;
                ShootSpikeInDirection(direction, 90f);
            }

            // Wait for the specified interval before shooting again
            yield return new WaitForSeconds(shootInterval);
        }
    }
    
    void ShootSpikeInDirection(Vector2 shootDirection, float rotation)
    {
        // Create the spike object from the prefab at the plant's position
        GameObject spike = Instantiate(spikePrefab, transform.position, Quaternion.Euler(0f, 0f, rotation));

        // Set the spike's velocity to move in the determined direction
        Rigidbody2D rb = spike.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero; 
            rb.velocity = shootDirection * spikeSpeed;
        }
    }

}
