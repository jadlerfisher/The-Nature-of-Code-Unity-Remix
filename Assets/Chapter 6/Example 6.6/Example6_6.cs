using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example6_6 : MonoBehaviour
{
    // Get scene references:
    public GameObject vehiclePrefab;
    public Path6_6 path;
    // Get example parameters:
    public int vehicleCount;
    public float minSpeed, maxSpeed;

    private Vehicle6_6[] vehicles;
    private float screenLeft, screenRight;

    // Start is called before the first frame update
    void Start()
    {
        // Get the left and right screen bounds for screen-wrapping logic.
        screenLeft = Camera.main.ScreenToWorldPoint(Vector2.zero).x;
        screenRight = Camera.main.ScreenToWorldPoint(Vector2.right * Screen.width).x;

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
            if (vehicle.transform.position.x > screenRight)
            {
                vehicle.transform.position = new Vector3(
                    screenLeft,
                    vehicle.transform.position.y,
                    vehicle.transform.position.z
                );
            }
            // Attempt to follow the path.
            vehicle.FollowPath(path);
        }
    }
}
