using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject respawnPoint;
    public GameObject enemyTarget;
    public GameObject roadParent;
    public GameObject roadTile;
    public GameObject Waypoints;
    public GameObject waypoint;
    public GameObject EnemyBucket;


    public int speed = 5;

    private GameObject currentEnemy;
    public List<GameObject> Waypoints_list = new List<GameObject>();
    public List<GameObject> Enemy_list = new List<GameObject>();
    public List<GameObject> SelectableSpots = new List <GameObject>();


    private GameObject lastWaypoint;


    void Start()
    {
        foreach (Transform child in Waypoints.transform)
        {
            if (child.gameObject.CompareTag("WaypointTag"))
            {
                Waypoints_list.Add(child.gameObject);
                
            }
        }
        foreach (Transform child in EnemyBucket.transform)
        {
            if (child.gameObject.CompareTag("Enemy"))
            {
                Enemy_list.Add(child.gameObject);
            }
        }
        SetLastWaypoint();
        
        foreach (var waypoint in Waypoints_list)
        {
            waypoint.GetComponent<MeshRenderer>().enabled = false;
        }


        // End of the Start section
        
    }

    void Update()
    {

    }

    void SetLastWaypoint()
    {
        Vector3 etPosition = new Vector3(enemyTarget.transform.position.x, enemyTarget.transform.position.y, enemyTarget.transform.position.z);
        lastWaypoint = Instantiate(waypoint, etPosition, Quaternion.identity);
        lastWaypoint.transform.SetParent(Waypoints.transform, true);
        Waypoints_list.Add(lastWaypoint);

    }

    public GameObject SpawnEnemy()
    {
        Vector3 spawnPos = respawnPoint.transform.position + new Vector3(0, 0, 1.6f);
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        currentEnemy = enemy;
        return enemy;
    }



}
