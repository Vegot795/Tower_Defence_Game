using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : MonoBehaviour, IDamager
{
    private List<GameObject> waypoints;
    private GameObject enemyTarget;
    private GameLogic gameLogic;
    private int currentWaypointIndex = 0;
    
    public float health = 20f;
    public float speed = 5f;
    public int scoreAmount = 30;

    ScoreManager scoreManager;
    void Start()
    {
        // Find the GameLogic instance
        gameLogic = FindObjectOfType<GameLogic>();

        if (gameLogic != null)
        {
            // Access waypoints and enemyTarget from GameLogic
            waypoints = gameLogic.Waypoints_list;
            enemyTarget = gameLogic.enemyTarget;

            Debug.Log("Enemy initialized with GameLogic waypoints and target.");
        }
        else
        {
            Debug.LogError("GameLogic script not found.");
        }
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (waypoints != null && waypoints.Count > 0)
        {
            if (currentWaypointIndex < waypoints.Count)
            {
                Vector3 targetPosition = waypoints[currentWaypointIndex].transform.position;
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    speed * Time.deltaTime
                );

                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    Debug.Log("Reached waypoint " + currentWaypointIndex);
                    currentWaypointIndex++;
                }
            }
            else if (enemyTarget != null)
            {
                Vector3 targetPosition = enemyTarget.transform.position;
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    speed * Time.deltaTime
                );
                Debug.Log("Moving towards enemy target.");
            }
            else
            {
                Debug.LogWarning("No enemy target assigned.");
            }
        }
        else
        {
            Debug.LogWarning("No waypoints assigned.");
        }
    }

    public void ReceiveDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        scoreManager.AddScore(scoreAmount);
        scoreManager.AddCurrency(scoreAmount);
        Destroy(gameObject);
    }
}
