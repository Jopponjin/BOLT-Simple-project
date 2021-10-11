using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShoot : MonoBehaviour
{
    public float startVelocity;
    public Vector3 startPosition;
    public Vector3 currentPosition;

    Transform BallisticVeloctiy(Vector3 from, Vector3 to, float gravity = 9.8f, float heightOff = 0.0f, float rangeOff = 0.11f)
    {
        Vector3 newVel = new Vector3();
        Vector3 direction  = from - transform.position;

        float range = direction.magnitude;
        Vector3 unitDirection = direction.normalized;

        float maxYPos = to.y + heightOff;

        if (maxYPos < from.y)
            maxYPos = from.y;
        
        // Find the total time by adding up the parts of the trajectory time to reach the max
        // The parts of the trajectory that the total time of discovery adds up Maximum time
        float floatTime;

        floatTime = -2.0f * gravity * (maxYPos - from.y);
        if (floatTime < 0) floatTime = 0f;
        newVel.y = Mathf.Sqrt(floatTime);

        floatTime = -2.0f * (maxYPos - from.y) / gravity;
        if (floatTime < 0)
            floatTime = 0f;

        float timeToMax = Mathf.Sqrt(floatTime);

        //Time to return to y-target
        floatTime = -2.0f * (maxYPos - to.y) / gravity;
        if (floatTime < 0)
            floatTime = 0f;

        float timeToTargetY = Mathf.Sqrt(floatTime);

        float totalFlightTime;
        totalFlightTime = timeToMax + timeToTargetY;

        // find the magnitude of the initial velocity in the xz direction
        // The magnitude of the initial velocity of the search is in the XZ direction
        float horizontalVelocityMagnitude = range / totalFlightTime;





        return null;
    }

    private void DrawTrajectory()
    {
        var curverPoints = new Vector3[0];
        curverPoints[0] = startPosition;

        var currentPosition = startPosition;
        var currentVelocity = startVelocity;


    }
}
