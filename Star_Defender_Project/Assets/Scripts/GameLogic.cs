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
    public GameObject currentEnemy;

    public TextMeshProUGUI SpawnEnemyButton;

    public int speed = 5;

    public List<GameObject> Waypoints_list = new List<GameObject>();
    public List<GameObject> Enemy_list = new List<GameObject>();
    public List<GameObject> SelectableSpots = new List <GameObject>();


    private GameObject lastWaypoint;


    void Start()
    {
        waypoint.GetComponent<MeshRenderer>().enabled = false;
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
        //DetectBlockNextToRoad();


        // End of the Start section
        
    }

    void Update()
    {

    }

    void SetLastWaypoint()
    {
        Vector3 etPosition = new Vector3(enemyTarget.transform.position.x, enemyTarget.transform.position.y, enemyTarget.transform.position.z -1);
        lastWaypoint = Instantiate(waypoint, etPosition, Quaternion.identity);
        lastWaypoint.transform.SetParent(Waypoints.transform, true);
        Waypoints_list.Add(lastWaypoint);

    } 

    public void SpawnEnemy()
    {
        currentEnemy = Instantiate(enemyPrefab, respawnPoint.transform.position, Quaternion.identity);
    }

    
}
