using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class TurretLogic : MonoBehaviour
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
            AimAtEnemy();

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
            if(!EnemiesInRange.Contains(enemy))
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
    void AimAtEnemy()
    {
        if(targetEnemy != null)
        {
            Vector3 direction = (targetEnemy.position - transform.position).normalized;
            float angleZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angleZ);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
    void Shoot()
    {
        if (fireCountdown <= 0f)
        {
            if (targetEnemy == null) return;
            GameObject missileGo = Instantiate(missilePrefab, firePoint.position, firePoint.rotation);
            MissleLogic missile = missileGo.GetComponent<MissleLogic>();

            if (missile != null)
            {
                missile.SetTarget(targetEnemy.gameObject); // Pass the target enemy
                missile.SetDamage(turretDamage);
            }

            Rigidbody rb = missileGo.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = (targetEnemy.position - firePoint.position).normalized;
                rb.velocity = direction * missileSpeed;
            }
        }

    }
   
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
