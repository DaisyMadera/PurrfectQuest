using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePlantShooter : MonoBehaviour
{
    public GameObject spikePrefab;
    public float shootInterval = 1f;
    public float spikeSpeed = 8f;
    public int projectileCount = 8; // Number of projectiles to shoot

    private void Start()
    {
        StartCoroutine(ShootSpikes());
    }

    private IEnumerator ShootSpikes()
    {
        while (true)
        {
            ShootSpikesInAllDirections();
            yield return new WaitForSeconds(shootInterval);
        }
    }

    private void ShootSpikesInAllDirections()
    {
        float angleIncrement = 360f / projectileCount; // Divide 360° into equal parts
        float startingAngle = 0f; // Starting angle for the first projectile

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = startingAngle + (i * angleIncrement);
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            ShootSpikeInDirection(direction, angle);
        }
    }

    private void ShootSpikeInDirection(Vector2 shootDirection, float rotation)
    {
        // Create the spike object from the prefab at the plant's position
        GameObject spike = Instantiate(spikePrefab, transform.position, Quaternion.Euler(0f, 0f, rotation));

        // Set the spike's velocity to move in the determined direction
        Rigidbody2D rb = spike.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = shootDirection * spikeSpeed;
        }
    }
}
