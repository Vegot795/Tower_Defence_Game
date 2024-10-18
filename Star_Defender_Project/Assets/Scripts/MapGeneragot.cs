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
    public GameObject mapTiles;
    public GameObject roadTIles;

    private List<GameObject> MTList = new List<GameObject> ();
    private List<GameObject> BTList = new List<GameObject> () ;
    
    

    // Start is called before the first frame update
    void Start()
    {
        
        MapGenerator();
        BorderCubeFounder();
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
                GameObject newMTile = Instantiate(mapTile, tilePosition, Quaternion.identity); // Changed to 'road'

                newMTile.transform.SetParent(mapTiles.transform);
                newMTile.transform.localPosition = tilePosition;
                MTList.Add(newMTile);
            }
        }
    }
    
    private void BorderCubeFounder()
    {
        foreach (var newTile in MTList)
        {
            if (newTile.transform.position.x == 0 && mapTile.transform.position.x == xMap )
            {
                BTList.Add(mapTile);
            }
            if (newTile.transform.position.y == 0 && mapTile.transform.position.y == yMap)
            {
                BTList.Add(mapTile);
            }
            
        }
        Debug.Log($"Attached {BTList.Count} to BTList");
    }
}
