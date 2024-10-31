using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneragot : MonoBehaviour
{
    public GameObject mapTile;
    public GameObject Waypoint;
    public int xMap = 16;
    public int yMap = 8;
    public GameObject mapTiles;
    public GameObject BorSign;
    public  List<GameObject> BTList = new List<GameObject>();


    private List<GameObject> MTList = new List<GameObject>();


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
            if (newTile.transform.localPosition.y == 0 && newTile.transform.localPosition.x < xMap)
            {
                BTList.Add(newTile);
            }
            if (newTile.transform.localPosition.y == yMap - 1 && newTile.transform.localPosition.x < xMap)
            {
                BTList.Add(newTile);
            }
            if (newTile.transform.localPosition.x == 0 && newTile.transform.localPosition.y < yMap)
            {
                BTList.Add(newTile);
            }
            if (newTile.transform.localPosition.x == xMap - 1 && newTile.transform.localPosition.y < yMap)
            {
                BTList.Add(newTile);
            }
        }
        Debug.Log($"Attached {BTList.Count} to BTList");
    }
}
