using UnityEngine;

public class WaypointNode : MonoBehaviour
{
    public float MaxSpeed; 

    public float MinDistanceToWaypoint = 5;

    public WaypointNode[] NextWaypoint;
}
