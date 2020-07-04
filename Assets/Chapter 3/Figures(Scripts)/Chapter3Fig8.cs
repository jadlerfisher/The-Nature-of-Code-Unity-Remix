using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig8 : MonoBehaviour
{
    // Get values from the inspector
    public float waveSpeed = 1;
    public float period = 10;
    public float amplitude = 0.5f;
    public GameObject waverPrefab;
    public int amountWavers;

    private float startAngle = 0f;
    private List<Transform> waveTransforms;

    // Start is called before the first frame update
    void Start()
    {
        // Populate the wave objects
        waveTransforms = new List<Transform>();
        while (waveTransforms.Count < amountWavers)
        {
            GameObject newWaver = Instantiate(waverPrefab);
            waveTransforms.Add(newWaver.transform);
        }

        // Get the edges of the screen from the scene camera
        Vector2 camTopRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 camBottomLeft = Camera.main.ScreenToWorldPoint(Vector2.zero);
        float top = camTopRight.y;
        float bottom = camBottomLeft.y;
        float left = camBottomLeft.x;
        float right = camTopRight.x;

        // Advance the wave relative to time
        startAngle += waveSpeed * Time.deltaTime;

        // Calculate the position of each object in the wave
        float currentAngle = startAngle;
        float currentX = left;
        foreach (Transform waveT in waveTransforms)
        {
            // Step along the circle, a larger period means steps are smaller
            currentAngle += 1 / period;

            // Remap the sin function so that y(-1, 1) corresponds to y(bottom, top)

            float currentY = Mathf.Lerp(bottom, top, Mathf.InverseLerp(-1f, 1f, Mathf.Sin(currentAngle)));

            waveT.position = new Vector2(currentX, currentY);
            // Step along the screen width such that every waver is on screen
            currentX += (right - left) / amountWavers;
        }
    }
}
