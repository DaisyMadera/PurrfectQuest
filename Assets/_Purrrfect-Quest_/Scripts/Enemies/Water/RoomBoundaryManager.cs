using UnityEngine;

public class RoomBoundaryManager : MonoBehaviour
{
    // The BoxCollider2D representing the room boundary
    public BoxCollider2D roomBoundaryCollider;

    private void Awake()
    {
        // Ensure that the Room Boundary Collider is assigned at runtime
        if (roomBoundaryCollider == null)
        {
            roomBoundaryCollider = GetComponent<BoxCollider2D>();
        }
    }
}