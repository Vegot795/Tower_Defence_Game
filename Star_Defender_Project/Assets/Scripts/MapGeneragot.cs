using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneragot : MonoBehaviour
{
    public GameObject mapTile;
    public GameObject road;
    public GameObject Waypoint;
    public int xMap = 16;
    public int yMap = 8;

    // Start is called before the first frame update
    void Start()
    {
        MapGenerator(); // Call the map generator here if you want it to run on start
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RoadGenerator()
    {
        // Add your road generation logic here
    }

    private void MapGenerator()
    {
        for (int i = 0; i < xMap; i++)
        {
            for (int j = 0; j < yMap; j++) // Corrected condition
            {
                Vector3 tilePosition = new Vector3(i, j, 0); // Fixed Vector3 initialization

                // Instantiate the road prefab or whichever prefab you want
                Instantiate(mapTile, tilePosition, Quaternion.identity); // Changed to 'road'
            }
        }
    }
}
