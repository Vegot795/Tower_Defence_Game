using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
internal class CubeData
{
    public Vector3 position;
    public string type;

    // Constructor for CubeData
    public CubeData(Vector3 pos, string cubeType)
    {
        position = pos;
        type = cubeType;
    }
}

[System.Serializable]
internal class MapData
{
    public List<CubeData> cubes = new List<CubeData>();
}
// - - - - - - - - - - - - - - - - - - CODE - CODE - CODE - - -
public class MapSaver : MonoBehaviour
{
    public List<GameObject> roadCubes;
    public List<GameObject> blockedCubes;
    public GameObject roadCubePrefab;
    public GameObject blockedCubePrefab;
    public RoadBuilder roadBuilder;
    

    private string savePath;

    private void Start()
    {
        //roadCubes = 
        // Default save path, can be updated with a slot index in SaveMap/LoadMap
        //savePath = Path.Combine(Application.persistentDataPath, "mapData.json");

        //Debug.Log("Persistent Data Path: " + Application.persistentDataPath);

    }

    private void GetLists() {
        List<GameObject> roadCubes = roadBuilder.RoadList;
        List<GameObject> blockedCubes =  roadBuilder._BTList;
    }
    public void SaveMap(int slotIndex)
    {
        // Update savePath for the selected save slot
        savePath = Path.Combine(Application.persistentDataPath, $"mapDataSlot{slotIndex}.json");

        MapData mapData = new MapData();

        // Collect data from road cubes
        foreach (GameObject cube in roadCubes)
        {
            CubeData cubeData = new CubeData(cube.transform.position, "road");
            mapData.cubes.Add(cubeData);
        }

        // Collect data from blocked cubes
        foreach (GameObject cube in blockedCubes)
        {
            CubeData cubeData = new CubeData(cube.transform.position, "blocked");
            mapData.cubes.Add(cubeData);
        }

        // Serialize to JSON and save to file
        string json = JsonUtility.ToJson(mapData, true);
        File.WriteAllText(savePath, json);

        Debug.Log($"Map saved to slot {slotIndex}: {savePath}");
    }

    public void LoadMap(int slotIndex)
    {
        // Update savePath for the selected save slot
        savePath = Path.Combine(Application.persistentDataPath, $"mapDataSlot{slotIndex}.json");

        if (!File.Exists(savePath))
        {
            Debug.LogWarning($"Save file not found at slot {slotIndex}: {savePath}");
            return;
        }

        string json = File.ReadAllText(savePath);
        MapData mapData;

        // Error handling for JSON parsing
        try
        {
            mapData = JsonUtility.FromJson<MapData>(json);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load map data: {e.Message}");
            return;
        }

        // Clear existing cubes from the scene
        foreach (GameObject cube in roadCubes) Destroy(cube);
        foreach (GameObject cube in blockedCubes) Destroy(cube);

        roadCubes.Clear();
        blockedCubes.Clear();

        // Instantiate saved cubes
        foreach (CubeData cubeData in mapData.cubes)
        {
            GameObject newCube = null;

            if (cubeData.type == "road")
            {
                // Safety check for road prefab
                if (roadCubePrefab == null)
                {
                    Debug.LogError("RoadCubePrefab is not assigned in the Inspector.");
                    continue;
                }
                newCube = Instantiate(roadCubePrefab, cubeData.position, Quaternion.identity);
                roadCubes.Add(newCube);
            }
            else if (cubeData.type == "blocked")
            {
                // Safety check for blocked prefab
                if (blockedCubePrefab == null)
                {
                    Debug.LogError("BlockedCubePrefab is not assigned in the Inspector.");
                    continue;
                }
                newCube = Instantiate(blockedCubePrefab, cubeData.position, Quaternion.identity);
                blockedCubes.Add(newCube);
            }

            // Log error if instantiation failed
            if (newCube == null)
            {
                Debug.LogError($"Failed to instantiate a cube of type '{cubeData.type}'. Check prefab assignments.");
            }
        }

        Debug.Log($"Map loaded from slot {slotIndex}: {savePath}");
    }
}
