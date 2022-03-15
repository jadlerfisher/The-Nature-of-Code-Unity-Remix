using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig9 : MonoBehaviour
{
    // Get values from the inspector
    [SerializeField] float waveSpeed = 1;
    [SerializeField] float period = 10;
    [SerializeField] float amplitude = 0.5f;
    [SerializeField] GameObject waverPrefab;
    [SerializeField] int amountWavers;

    private float startAngle = 0f;
    private List<Transform> waveTransforms;

    private Vector2 maximumPos;

    // Start is called before the first frame update
    void Start()
    {
        // Populate the wave objects
        waveTransforms = new List<Transform>();
        while(waveTransforms.Count < amountWavers)
        {
            GameObject newWaver = Instantiate(waverPrefab);
            waveTransforms.Add(newWaver.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        FindWindowLimits();

        // Advance the wave relative to time
        startAngle += waveSpeed * Time.deltaTime;

        // Calculate the position of each object in the wave
        float currentAngle = startAngle;
        float currentX = -maximumPos.x;
        foreach(Transform waveTransform in waveTransforms)
        {
            // Step along the circle, a larger period means steps are smaller
            currentAngle += 1 / period;

            // Remap the sin function so that y(-1, 1) corresponds to y(bottom, top)
            float currentY =  Mathf.Lerp(-maximumPos.y, maximumPos.y, Mathf.InverseLerp(-1f, 1f, Mathf.Sin(currentAngle)));

            // Set the position of each wave transform
            waveTransform.position = new Vector2(currentX, currentY);

            // Step along the screen width such that every waver is on screen
            currentX += (maximumPos.x - -maximumPos.x) / amountWavers;
        }
    }

    private void FindWindowLimits()
    {
        // Start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;
        // For correct functionality, we set the camera x and y position to 0, 0
        Camera.main.transform.position = new Vector3(0, 0, -10);
        // Next we grab the minimum and maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}