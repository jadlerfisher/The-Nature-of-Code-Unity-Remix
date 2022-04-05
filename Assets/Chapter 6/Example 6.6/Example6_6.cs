using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example6_6 : MonoBehaviour
{
    // Get scene references:
    [SerializeField] GameObject vehiclePrefab;
    [SerializeField] Path6_6 path;
    // Get example parameters:
    [SerializeField] int vehicleCount;
    [SerializeField] float minSpeed, maxSpeed;

    private Vehicle6_6[] vehicles;
    private Vector3 maximumPos;

    // Start is called before the first frame update
    void Start()
    {
        FindWindowLimits();

        // Spawn the vehicles into the scene.
        vehicles = new Vehicle6_6[vehicleCount];
        for (int i = 0; i < vehicleCount; i++)
        {
            // Create a new vehicle in front of the track.
            GameObject vehicle = Instantiate(vehiclePrefab, Vector3.back, Quaternion.identity);

            // Assign initial velocity and max speed values for each vehicle.
            vehicles[i] = vehicle.GetComponent<Vehicle6_6>();
            vehicles[i].body.velocity = Vector3.right * 10;
            vehicles[i].maxspeed = Random.Range(minSpeed, maxSpeed);
        }
    }

    // FixedUpdate is called once per physics step
    void FixedUpdate()
    {
        foreach (Vehicle6_6 vehicle in vehicles)
        {
            // When the vehicle passes the right edge, wrap back to the left edge.
            if (vehicle.transform.position.x > maximumPos.x)
            {
                vehicle.transform.position = new Vector3(
                    -maximumPos.x,
                    vehicle.transform.position.y,
                    vehicle.transform.position.z
                );
            }
            // Attempt to follow the path.
            vehicle.FollowPath(path);
        }
    }

    private void FindWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;

        // For FindWindowLimits() to function correctly, the camera must be set to coordinates 0, 0, -10
        Camera.main.transform.position = new Vector3(0, 0, -5);

        // Next we grab the maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}
