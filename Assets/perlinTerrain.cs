using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class perlinTerrain : MonoBehaviour
{
    public List<Vector3> terrainArray = new List<Vector3>();
    public GameObject terrainCube;
    public int cols, rows;
    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;
    public Color color5;
    public Color color6;





    // Start is called before the first frame update
    void Start()
    {
        GameObject terrain = new GameObject();
        terrain.name = "terrain";

        float xoff = 0;
        for (int i = 0; i < cols; i++)
        {
            float yoff = 0;
            for (int j = 0; j < rows; j++)
            {
                
                float theta = ExtensionMethods.Remap(Mathf.PerlinNoise(xoff, yoff), 0f, 1f, 0f, 10f);

                terrainCube = Instantiate(terrainCube, new Vector3(i, theta, j), Quaternion.identity);
                terrainCube.transform.SetParent(terrain.transform);
                Renderer terrainRenderer = terrainCube.GetComponent<Renderer>();
                terrainRenderer.material.SetColor("_Color", colorTerrain(terrainCube.transform.position));
                
                yoff += 0.08f;
            }
            xoff += 0.08f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public Color colorTerrain (Vector3 terrainCubePosition)
    {
        Color terrainColor = new Vector4(1f, 1f, 1f);
        if (terrainCubePosition.y >= 0f && terrainCubePosition.y <= 2f)
        {
            terrainColor = color1;
        }
        else if (terrainCubePosition.y >= 2f && terrainCubePosition.y <= 3.5f)
        {
            terrainColor = color2;
        }
        else if (terrainCubePosition.y >= 3.5f && terrainCubePosition.y <= 4.5f)
        {
            terrainColor = color3;
        }
        else if (terrainCubePosition.y >= 4.5f && terrainCubePosition.y <= 5f)
        {
            terrainColor = color4;
        }
        else if (terrainCubePosition.y >= 5f && terrainCubePosition.y <= 6f)
        {
            terrainColor = color5;
        }
        else if (terrainCubePosition.y >= 6f)
        {
            terrainColor = color6;
        }

        return terrainColor;
    }

}
