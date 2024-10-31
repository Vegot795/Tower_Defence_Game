using System.Collections.Generic;
using UnityEngine; // Add this to access Unity types like Vector3

[System.Serializable]
public class TileData
{
    public Vector3 position; // Position of the tile
    public string type;      // Type of the tile (e.g., "Road", "Turret")
    public float rotation;   // Rotation in degrees
}

[System.Serializable]
public class MapData
{
    public List<TileData> tiles = new List<TileData>(); // List of all tiles on the map
}
