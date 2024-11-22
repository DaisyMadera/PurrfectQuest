using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int targetPoint;
    public float speed;
    public float detectionRange;
    public Transform Player; // Reference to the player
    private Animator animator; // Reference to the Animator component

    void Start()
    {
        targetPoint = 0;
        animator = GetComponent<Animator>(); // Assume the Animator is attached to the same GameObject
    }

    void Update()
    {
        // Check if the player is within range
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);
        if (distanceToPlayer <= detectionRange)
        {
            // Change target point to the second point
            targetPoint = 1;

            // Trigger the new animation
            animator.SetTrigger("Mouse_IdleLeft"); // Assumes you have an animation trigger named "RunAway"
        }

        // Move towards the target point
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[targetPoint].position, speed * Time.deltaTime);

        // Check if the mouse reached the target point
        //if (transform.position == patrolPoints[targetPoint].position)
        //{
        //    increaseTargetInt();
        //}
    }
    void increaseTargetInt(){
        targetPoint++;
        if(targetPoint >= patrolPoints.Length){
            targetPoint = 0;
        }
    }
}
