using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BturretLogic : MonoBehaviour
{
    public float turretDamage = 500;
    public GameObject missilePrefab;
    public float range = 20f;
    public float fireRate = 1f;
    public Transform firePoint;
    public float missileSpeed = 20f;
    public float rotationSpeed = 50f;
    public int level = 1;
    public Transform targetEnemy;
    public BTAmmoLogic BTAmmoLogic; 

    private float fireCountdown;
    private List<GameObject> EnemiesInRange = new List<GameObject>();
    private int cost = 100;
    private int afterSellCurrency;
    private int upgradeCost;
    private int currentValue;

    ScoreManager scoreManager;

    private int GetUpgradeCost()
    {
        if (level == 1)
        {
            upgradeCost = cost + (cost / 2);
        }
        else if (level < 5)
        {
            upgradeCost = upgradeCost + (upgradeCost / 2);
        }
        return upgradeCost;
    }
    
    private void GetAllComponents()
    {
        scoreManager = GetComponent<ScoreManager>();
        currentValue = cost;
        GetUpgradeCost();
    }
    private void Start()
    {
        Physics.gravity = new Vector3(0, 0, 9.81f); // Set gravity to -Z direction
        fireCountdown = 1f / fireRate;
        GetAllComponents();

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
        // Remove destroyed enemies from the list
        EnemiesInRange.RemoveAll(enemy => enemy == null);

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
        BTAmmoLogic = BTAmmo.GetComponent<BTAmmoLogic>();
        if (BTAmmoLogic != null)
        {
            BTAmmoLogic.SetTarget(targetEnemy);
            BTAmmoLogic.SetDamage(turretDamage);
        }

        if (rb != null)
        {
            Vector3 start = firePoint.position;
            Vector3 end = targetEnemy.position;

            float h = 5f; // dodatkowy ³uk wysokoœci
            float gravity = Mathf.Abs(Physics.gravity.z); // 9.81

            // Pozioma odleg³oœæ w XY
            Vector3 horizontal = new Vector3(end.x - start.x, end.y - start.y, 0f);
            float horizontalDistance = horizontal.magnitude;

            // Prêdkoœæ pionowa (w osi Z) do osi¹gniêcia h
            float vz = Mathf.Sqrt(2 * gravity * h);

            // Czas w górê i w dó³
            float timeUp = vz / gravity;
            float heightDifference = start.z - end.z; // UWAGA: start - end, bo Z dzia³a odwrotnie
            float timeDown = Mathf.Sqrt(2 * Mathf.Max(0f, h + heightDifference) / gravity);
            float totalTime = timeUp + timeDown;

            // Prêdkoœæ pozioma
            Vector3 velocityXY = horizontal / totalTime;

            // Pe³na prêdkoœæ
            Vector3 launchVelocity = new Vector3(velocityXY.x, velocityXY.y, -vz); // MINUS, bo -Z to góra
            rb.velocity = launchVelocity;

            Debug.Log("Pocisk wystrzelony z prêdkoœci¹: " + rb.velocity);
        }
    }





    public int GetLevel()
    {
        return level;
    }
    public void Upgrade()
    {
        if (level < 5)
        {
            if (scoreManager.currency > upgradeCost)
            {
                level++;
                turretDamage = turretDamage * 1.5f;
                range = range * 1.25f;
                scoreManager.currency -= upgradeCost;
                currentValue += upgradeCost;
            
                Debug.Log("Tower upgraded to level " + level);
            }
        }
        else
        {
            Debug.Log("Max level reached");
        }

    }
    public void Sell()
    {
        afterSellCurrency = currentValue / 3;
        Debug.Log("Tower Sold For " + afterSellCurrency);
        scoreManager.AddCurrency(afterSellCurrency);
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
