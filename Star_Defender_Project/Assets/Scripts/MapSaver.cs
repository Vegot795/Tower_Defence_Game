using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
internal class CubeData
{
    public Vector3 position;
    public string type;

    // Corrected constructor (no void return type)
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

public class MapSaver : MonoBehaviour
{
    public List<GameObject> roadCubes;
    public List<GameObject> blockedCubes;
    private string savePath;

    private void Start()
    {
        // Corrected to use Path.Combine
        savePath = Path.Combine(Application.persistentDataPath, "mapData.json");
    }

    public void SaveMap()
    {
        MapData mapData = new MapData();

        foreach (GameObject cube in roadCubes)
        {
            CubeData cubeData = new CubeData(cube.transform.position, "road");
            mapData.cubes.Add(cubeData);
        }

        foreach (GameObject cube in blockedCubes) // Corrected GameObject type
        {
            CubeData cubeData = new CubeData(cube.transform.position, "blocked");
            mapData.cubes.Add(cubeData);
        }

        string json = JsonUtility.ToJson(mapData, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Map saved to " + savePath);
    }

    public void LoadMap()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("Save file not found at " + savePath);
            return;
        }

        string json = File.ReadAllText(savePath);
        MapData mapData = JsonUtility.FromJson<MapData>(json);

        // Clear existing cubes
        foreach (GameObject cube in roadCubes) Destroy(cube);
        foreach (GameObject cube in blockedCubes) Destroy(cube);

        roadCubes.Clear();
        blockedCubes.Clear();

        // Corrected to iterate over mapData.cubes
        foreach (CubeData cubeData in mapData.cubes)
        {
            GameObject newCube;

            if (cubeData.type == "road")
            {
                newCube = Instantiate(Resources.Load<GameObject>("RoadCubePrefab"), cubeData.position, Quaternion.identity);
                roadCubes.Add(newCube); // Add to roadCubes
            }
            else if (cubeData.type == "blocked")
            {
                newCube = Instantiate(Resources.Load<GameObject>("BlockedCubePrefab"), cubeData.position, Quaternion.identity);
                blockedCubes.Add(newCube); // Add to blockedCubes
            }
        }

        Debug.Log("Map loaded from " + savePath);
    }
}
