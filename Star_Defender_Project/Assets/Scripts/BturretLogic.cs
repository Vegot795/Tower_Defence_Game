using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BturretLogic : MonoBehaviour
{
    public float turretDamage = 5f;
    public GameObject missilePrefab;
    public float range = 10f;
    public float fireRate = 1f;
    public Transform firePoint;
    public float missileSpeed = 20f;
    public float rotationSpeed = 50f;

    private float fireCountdown;
    private Transform targetEnemy;
    private List<GameObject> EnemiesInRange = new List<GameObject>();

    private void Start()
    {
        fireCountdown = 1f / fireRate;

    }

    private void Update()
    {

        FindTarget();


        if (targetEnemy != null)
        {

            // Decrement fireCountdown and shoot if needed
            fireCountdown -= Time.deltaTime;
            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate; // Reset fireCountdown
            }
        }
    }

    void FindTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        HashSet<GameObject> currentEnemies = new HashSet<GameObject>();

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                currentEnemies.Add(collider.gameObject);
            }
        }
        foreach (GameObject enemy in EnemiesInRange)
        {
            if (!currentEnemies.Contains(enemy))
            {
                EnemiesInRange.Remove(enemy);
                break;
            }
        }
        foreach (GameObject enemy in currentEnemies)
        {
            if (!EnemiesInRange.Contains(enemy))
            {
                EnemiesInRange.Add(enemy);
            }
        }
        if (EnemiesInRange.Count > 0)
        {
            targetEnemy = EnemiesInRange.First().transform;
        }
        else
        {
            targetEnemy = null;
        }
    }
    
    void Shoot()
    {
        if (targetEnemy == null)
            return;
        GameObject BTAmmo = Instantiate(missilePrefab, firePoint.position, Quaternion.identity);

        Rigidbody rb = BTAmmo.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 direction = (targetEnemy.position - firePoint.position).normalized;
            float upwardArcHeight = 5f;
            float distanceToEnemy = Vector3.Distance(firePoint.position, targetEnemy.position);

            Vector3 velocity = new Vector3(
                direction.x * distanceToEnemy / 2,
                direction.y * distanceToEnemy / 2,
                -upwardArcHeight
                ).normalized * missileSpeed;

            rb.velocity = velocity;
            
        }
        Debug.Log("BTAmmo fired");

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
