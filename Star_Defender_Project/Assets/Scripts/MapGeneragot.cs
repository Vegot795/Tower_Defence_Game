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

    private int MaxPathLength = 32;
    
    

    // Start is called before the first frame update
    void Start()
    {
        
        MapGenerator();
        BorderCubeFounder();
        GetStartendCubes();
    }

    // Update is called once per frame
    void Update()
    {

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
            Vector3 tilePos = newTile.transform.position;

            // Check if the tile is on the border of the map (left, right, top, or bottom edges)
            if (tilePos.x == 0 || tilePos.x == xMap - 1 || tilePos.y == 0 || tilePos.y == yMap - 1)
            {
                BTList.Add(newTile);
            }
        }
        Debug.Log($"Attached {BTList.Count} border tiles to BTList");
    }


    GameObject GetRandomBC()
    {
        if (BTList.Count == 0)
        {
            Debug.Log("System found no object in BTList");
            return null;
        }
        int randomIndex = Random.Range(0, BTList.Count);


        return BTList[randomIndex];
        
    }
    private void GetStartendCubes()
    {
        GameObject startCube = GetRandomBC();
        GameObject endCube = GetRandomBC();

        // Ensure the end cube is different from the start cube
        while (startCube != null && startCube == endCube)
        {
            endCube = GetRandomBC();
        }

        // Check if startCube and endCube are valid
        if (startCube == null || endCube == null)
        {
            Debug.LogError("Failed to select start or end cube.");
            return;
        }

        Debug.Log($"Start Cube: {startCube.transform.position}, End Cube: {endCube.transform.position}");

        // Now generate the path
        List<GameObject> path = RoadGenerator(startCube, endCube);

        if (path.Count > 0)
        {
            Debug.Log($"Generated a path with {path.Count} tiles.");

            // Visualize the path by instantiating road tiles along the path
            foreach (var cube in path)
            {
                Instantiate(road, cube.transform.position, Quaternion.identity, roadTIles.transform);
            }
        }
        else
        {
            Debug.LogWarning("No path generated.");
        }
    }

    private List<GameObject> RoadGenerator(GameObject startCube, GameObject endCube)
    {
        List<GameObject> path = new List<GameObject>();

        if (!MTList.Contains(startCube) || !MTList.Contains(endCube))
        {
            Debug.LogWarning("Start or End cube not found in MTList");
            return path;
        }

        path.Add(startCube);
        GameObject currentCube = startCube;

        Debug.Log("Starting path generation...");

        while (currentCube != endCube && path.Count < MaxPathLength)
        {
            List<GameObject> neighbors = GetNeighbors(currentCube);

            if (neighbors.Count == 0)
            {
                Debug.LogWarning("No neighbors found. Breaking path generation.");
                break;
            }

            GameObject nextCube = null;

            foreach (var neighbor in neighbors)
            {
                if (nextCube == null || Vector3.Distance(neighbor.transform.position, endCube.transform.position) < Vector3.Distance(nextCube.transform.position, endCube.transform.position))
                {
                    nextCube = neighbor;
                }
            }

            if (nextCube == null || path.Contains(nextCube))
            {
                Debug.LogWarning("No valid next cube found or cube already in path. Breaking.");
                break;
            }

            currentCube = nextCube;
            path.Add(currentCube);
        }

        if (currentCube == endCube)
        {
            Debug.Log("Path successfully generated and reached the end cube.");
        }
        else
        {
            Debug.LogWarning("Path could not reach the end cube.");
        }

        return path;
    }


    private List<GameObject> GetNeighbors(GameObject cube)
    {
        List<GameObject> neighbors = new List<GameObject>();
        Vector3 cubePos = cube.transform.position;

        // Iterate through the MTList and find neighboring cubes (left, right, up, down)
        foreach (GameObject mt in MTList)
        {
            Vector3 mtPos = mt.transform.position;

            // Check for direct neighbors (left, right, up, down) based on position
            if ((mtPos.x == cubePos.x && Mathf.Abs(mtPos.y - cubePos.y) == 1) ||
                (mtPos.y == cubePos.y && Mathf.Abs(mtPos.x - cubePos.x) == 1))
            {
                neighbors.Add(mt);
            }
        }

        return neighbors;
    }

    private void RoadInstantiator()
    {
        foreach(var cube in path)
        {

        }
    }

}
