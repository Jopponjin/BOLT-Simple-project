using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShoot : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float startVelocity;
    public Vector3 currentPosition;

    
    Vector3[] curverPoints;

    float angle;
    public float velocity;
    public float rads;
    public int resolution = 5;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

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

    private void Update()
    {
        DrawTrajectory();
    }

    private void DrawTrajectory()
    {
        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, float.MaxValue, LayerMask.GetMask("Ground")))
            {
                angle = -gameObject.transform.rotation.eulerAngles.x;
                lineRenderer.positionCount = resolution + 1;
                lineRenderer.SetPositions(CalculateArcPoints());
            }
        }
        

    }

    Vector3 CalculatePoint(float t, float maxDist)
    {
        float x = t * maxDist;
        float y = x * Mathf.Tan(rads) - ((Physics.gravity.y * x * x) / (2 * Mathf.Pow(velocity * Mathf.Cos(rads), 2)));
        float z = 0f;

        return new Vector3(z, y, x);
    }

    Vector3[] CalculateArcPoints()
    {
        Vector3[] arcPoints = new Vector3[resolution + 1];

        rads = Mathf.Deg2Rad * angle;

        float maxDistance = ((velocity * Mathf.Cos(rads)) / Physics.gravity.y) * (velocity * Mathf.Sin(rads) + Mathf.Sqrt(Mathf.Pow(velocity * Mathf.Sin(rads), 2) + 2 * Physics.gravity.y * gameObject.transform.parent.position.y));

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            arcPoints[i] = CalculatePoint(t, maxDistance);
        }

        return arcPoints;
    }
}
