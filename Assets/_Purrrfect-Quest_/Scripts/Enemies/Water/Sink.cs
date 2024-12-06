using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class Sink : MonoBehaviour
{
    public Tilemap furnitureTilemap;  // Reference to the Tilemap for furniture (where the sink is)
    public TileBase sinkTile;         // The specific tile that represents the sink
    public GameObject dropletPrefab;  // The droplet prefab
    public float dropletForce = 5f;   // Force applied to the droplet
    public float dropletInterval = 3f; // Interval between droplet spawnings
    public GameObject player;         // Reference to the player GameObject
    public float spawnAheadDistance = 3f; // Distance ahead of player where droplet will spawn

    // Reference to the Room Boundary Collider
    public BoxCollider2D roomBoundaryCollider;

    private void Start()
    {
        // Start shooting droplets at regular intervals
        StartCoroutine(ShootDroplets());
    }

    private IEnumerator ShootDroplets()
    {
        // Continuously spawn droplets from the sink tile
        while (true)
        {
            SpawnDropletsFromSink();
            yield return new WaitForSeconds(dropletInterval);
        }
    }

    private void SpawnDropletsFromSink()
    {
        // Get the bounds of the furniture Tilemap and loop through each cell
        BoundsInt bounds = furnitureTilemap.cellBounds;

        foreach (var position in bounds.allPositionsWithin)
        {
            // Check if the current tile is the sink tile
            TileBase tile = furnitureTilemap.GetTile(position);
            if (tile == sinkTile)
            {
                // Convert the sink tile position to world space
                Vector3 spawnPosition = furnitureTilemap.CellToWorld(position);

                Vector3 playerVelocityDirection = player.GetComponent<Rigidbody2D>().velocity.normalized;  // Get player movement direction
                Vector3 spawnAheadPosition = spawnPosition + playerVelocityDirection * spawnAheadDistance;

                // Clamp spawn position to stay within the room boundary
                spawnAheadPosition.x = Mathf.Clamp(spawnAheadPosition.x, roomBoundaryCollider.bounds.min.x + 0.5f, roomBoundaryCollider.bounds.max.x -0.5f);
                spawnAheadPosition.y = Mathf.Clamp(spawnAheadPosition.y, roomBoundaryCollider.bounds.min.y + 0.5f, roomBoundaryCollider.bounds.max.y - 0.5f);

                if (IsPositionWithinRoomBoundary(spawnAheadPosition))
                {
                    SpawnDroplet(spawnAheadPosition);
                }
            }
        }
    }

    private bool IsPositionWithinRoomBoundary(Vector3 position)
    {
        // Check if the spawn position is inside the room boundary collider
        float tolerance = 0.01f;
        return roomBoundaryCollider.OverlapPoint(position)|| 
           (position.x > roomBoundaryCollider.bounds.min.x - tolerance && 
            position.x < roomBoundaryCollider.bounds.max.x + tolerance &&
            position.y > roomBoundaryCollider.bounds.min.y - tolerance &&
            position.y < roomBoundaryCollider.bounds.max.y + tolerance);
    }

    private void SpawnDroplet(Vector3 position)
    {
         // Instantiate the droplet at the specified position
        GameObject droplet = Instantiate(dropletPrefab, position, Quaternion.identity);
        if (dropletPrefab != null)
        {
            // Apply force in the player's direction
            Rigidbody2D rb = droplet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector3 directionToPlayer = (player.transform.position - position).normalized; // Direction to player
                rb.AddForce((Vector2)directionToPlayer * dropletForce, ForceMode2D.Impulse);

                // Calculate the rotation angle based on the direction to the player
                float rotationAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

                float defaultRotation = 90f;
                rotationAngle += defaultRotation;

                droplet.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
            
                rb.drag = 0.5f; // Lower drag value
                rb.gravityScale = 0.1f; // Slight gravity scale to slow droplet
            }
        
            
        }
    }
}