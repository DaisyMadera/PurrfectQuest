using UnityEngine;

public class Droplet : MonoBehaviour
{
    public GameObject puddlePrefab;    // Prefab for the puddle
    public float puddleLifetime = 0.2f;  // Lifetime of the puddle in seconds
    public float distanceThreshold = 20f;  // Distance traveled before turning into a puddle

    private Vector3 startPosition;  // Initial position of the droplet
    private bool hasTurnedIntoPuddle = false; // Flag to prevent creating multiple puddles

    private BoxCollider2D roomBoundaryCollider;  // Room boundary collider

    private void Awake()
    {
        // Try to find the RoomBoundaryManager in the scene
        RoomBoundaryManager boundaryManager = FindObjectOfType<RoomBoundaryManager>();

        if (boundaryManager != null)
        {
            // Get the BoxCollider2D from the RoomBoundaryManager
            roomBoundaryCollider = boundaryManager.GetComponent<BoxCollider2D>();
        }
    }

    private void Start()
    {
        startPosition = transform.position;  // Record the initial position
    }

    private void Update()
    {
        if (hasTurnedIntoPuddle) return;  // Skip if droplet has already turned into a puddle

        // Check if the droplet has traveled the specified threshold distance
        float distanceTraveled = Vector3.Distance(startPosition, transform.position);
        if (distanceTraveled >= distanceThreshold)
        {
            CreatePuddle();
        }

        if (roomBoundaryCollider != null)
        {
            // Get 2D position of the droplet
            Vector2 dropletPosition2D = new Vector2(transform.position.x, transform.position.y);

            // Get the 2D bounds of the collider
            Vector2 boundsMin = new Vector2(roomBoundaryCollider.bounds.min.x, roomBoundaryCollider.bounds.min.y);
            Vector2 boundsMax = new Vector2(roomBoundaryCollider.bounds.max.x, roomBoundaryCollider.bounds.max.y);

            if (dropletPosition2D.x < boundsMin.x || dropletPosition2D.x > boundsMax.x ||
            dropletPosition2D.y < boundsMin.y || dropletPosition2D.y > boundsMax.y)
            {
                Destroy(gameObject);
            }
           
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If collides with the ground, create the puddle
        if (collision.gameObject.CompareTag("Ground") && !hasTurnedIntoPuddle)
        {
            CreatePuddle();
        }
    }

    private void CreatePuddle()
    {
        // Instantiate the puddle at the droplet's current position
        Vector3 puddlePosition = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z); // Adjust puddle position above player’s path
        GameObject puddle = Instantiate(puddlePrefab, puddlePosition, Quaternion.identity);

        // Set the flag so puddle creation doesn't happen again
        hasTurnedIntoPuddle = true;

        // Destroy the droplet after turning into a puddle
        Destroy(gameObject);

        // Destroy the puddle after a certain lifetime
        Destroy(puddle, puddleLifetime);  // Destroy the puddle after `puddleLifetime` seconds
    }
}