using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle6_5 : MonoBehaviour
{
    // Variables accessible to other scripts.
    public float maxspeed;
    public Rigidbody2D body;

    // Update is called once per frame
    void Update()
    {
        // Look in the direction the vehicle is traveling in.
        // Vector3.back must be specified since that is the "up" direction in our scene.
        gameObject.transform.LookAt(body.position + body.velocity, Vector3.back);
    }

    public void Seek(Vector2 target)
    {
        // Get a vector pointing from our location to the target.
        Vector2 desired = target - body.position;

        // Scale our desired vector by our maximum speed.
        desired = desired.normalized * maxspeed;

        // Apply Reynold's path following force relative to time.
        Vector2 steer = desired - body.velocity;
        body.AddForce(steer * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }

    public void FollowPath(Path6_5 path)
    {
        // Predict the future location of the body.
        Vector2 predictedLocation = body.position + body.velocity.normalized * 2.5f;

        // Find the closest point along the path:
        Vector2 a = path.startVector.position;
        Vector2 b = path.endVector.position;
        Vector2 normalPoint = GetNormalPoint(predictedLocation, a, b);

        // Determine a follow target some distance further down the path.
        Vector2 pathDirection = b - a;
        Vector2 alongPath = pathDirection.normalized * 2.5f;
        Vector2 target = normalPoint + alongPath;

        // Is the vehicle predicted to leave the path?
        float distance = Vector2.Distance(normalPoint, predictedLocation);
        if(distance > path.radius) 
        {
            // If so, steer the vehicle towards the path.
            Seek(target);
        }

        #region Debug Line Drawing
        // Send the information that was calculated to the debug lines drawer.
        DrawDebugLines(
            (distance > path.radius) ? normalPoint : Vector2.zero,
            (distance > path.radius) ? target : Vector2.zero,
            body.position, predictedLocation,
            predictedLocation, normalPoint
        );
        #endregion
    }

    private Vector2 GetNormalPoint(Vector2 point, Vector2 start, Vector2 end)
    {
        // Treat start as the origin of our problem.
        Vector2 ap = point - start;
        Vector2 ab = end - start;

        // Scale the vector by the dot product to find the nearest point to p.
        ab.Normalize();
        ab *= Vector2.Dot(ap, ab);

        // Re-add the relative position of our input.
        Vector2 normalPoint = ab + start;
        return normalPoint;
    }

    #region Debug Line Drawing
    // Fields for debug:
    private bool debugIsActive = false;
    private LineRenderer predictLine, normalLine, steerLine;
    // Setup of the debug elements:
    void Start()
    {
        predictLine = new GameObject().AddComponent<LineRenderer>();
        predictLine.material = new Material(Shader.Find("Diffuse"));
        predictLine.positionCount = 2;
        predictLine.widthMultiplier = 0.1f;

        normalLine = new GameObject().AddComponent<LineRenderer>();
        normalLine.material = new Material(Shader.Find("Diffuse"));
        normalLine.positionCount = 2;
        normalLine.widthMultiplier = 0.1f;

        steerLine = new GameObject().AddComponent<LineRenderer>();
        steerLine.material = new Material(Shader.Find("Diffuse"));
        steerLine.positionCount = 2;
        steerLine.widthMultiplier = 0.1f;

        StartCoroutine(DebugUpdate());
    }
    // Check for the space bar to be pressed.
    private IEnumerator DebugUpdate()
    {
        while(true)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("yeet");
                debugIsActive = !debugIsActive;
                if(!debugIsActive)
                {
                    predictLine.SetPosition(0, Vector3.zero);
                    predictLine.SetPosition(1, Vector3.zero);
                    normalLine.SetPosition(0, Vector3.zero);
                    normalLine.SetPosition(1, Vector3.zero);
                    steerLine.SetPosition(0, Vector3.zero);
                    steerLine.SetPosition(1, Vector3.zero);
                }
            }
            yield return null;
        }
    }
    // Draw the debug lines.
    private void DrawDebugLines(Vector2 steerStart, Vector2 steerEnd, Vector2 predictStart,
                                Vector2 predictEnd, Vector2 normalStart, Vector2 normalEnd)
    {
        if(debugIsActive)
        {
            steerLine.SetPosition(0, new Vector3(steerStart.x, steerStart.y, -1));
            steerLine.SetPosition(1, new Vector3(steerEnd.x, steerEnd.y, -1));
            predictLine.SetPosition(0, new Vector3(predictStart.x, predictStart.y, -1));
            predictLine.SetPosition(1, new Vector3(predictEnd.x, predictEnd.y, -1));
            normalLine.SetPosition(0, new Vector3(normalStart.x, normalStart.y, -1));
            normalLine.SetPosition(1, new Vector3(normalEnd.x, normalEnd.y, -1));
        }
    }
    #endregion
}
