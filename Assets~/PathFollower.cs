using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 2.5f;
    [SerializeField] private float arrivalThreshold = 1f;
    
    // Higher values make the object snap faster; lower values make it turn slower
    [SerializeField] private float rotationSpeed = 5f; 

    private int currentWaypointIndex = 0;

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        if (targetWaypoint == null) return;

     transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

    if (Vector3.Distance(transform.position, targetWaypoint.position) < arrivalThreshold)
    {  
    Debug.Log("Reached waypoint: " + currentWaypointIndex);
    
    // Only move to the next index if we haven't reached the end of the array
    if (currentWaypointIndex < waypoints.Length - 1)
    {
        currentWaypointIndex++;
    }
    else
    {
        speed = 0f;
    }
    }

    // DEBUG: Show a line in the editor where the object wants to go
    Debug.DrawLine(transform.position, targetWaypoint.position, Color.red);

    if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
    {
        Debug.Log("Reached waypoint: " + currentWaypointIndex);
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    } 

    private void RotateTowardsTarget(Vector3 targetPosition)
    {
        // Calculate the direction vector from the object to the waypoint
        Vector3 direction = targetPosition - transform.position;

        // Safety check to ensure we don't try to look at an identical position
        if (direction != Vector3.zero)
        {
            // Create the target rotation looking in that direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Interpolate smoothly from current rotation to the target rotation
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                targetRotation, 
                rotationSpeed * Time.deltaTime
            );
        }
    }
    
private void OnDrawGizmos()
{
    // Don't draw if there are no waypoints assigned
    if (waypoints == null || waypoints.Length == 0) return;

    // Draw the path connecting all waypoints in Yellow
    Gizmos.color = Color.yellow;
    for (int i = 0; i < waypoints.Length; i++)
    {
        if (waypoints[i] == null) continue;
        
        int nextIndex = (i + 1) % waypoints.Length;
        if (waypoints[nextIndex] != null)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[nextIndex].position);
        }
    }

    // Draw a Red line from this object to its CURRENT active target
    if (Application.isPlaying && waypoints[currentWaypointIndex] != null)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, waypoints[currentWaypointIndex].position);
    }
}
 
}
