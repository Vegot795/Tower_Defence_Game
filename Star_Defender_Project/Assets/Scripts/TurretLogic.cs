using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class TurretLogic : MonoBehaviour
{
    public bool isInPreview = false;
    public float turretDamage = 5f;
    public GameObject missilePrefab;
    public float range = 10f;
    public float fireRate = 1f;
    public Transform firePoint;
    public float missileSpeed = 20f;
    public float rotationSpeed = 50f;
    public int level = 1;

    private Transform targetEnemy;
    private List<GameObject> EnemiesInRange = new List<GameObject>();
    private float fireCountdown;
    private int cost = 100;
    private int afterSellCurrency;
    private int upgradeCost;
    private int currentValue;
    private bool active = true;

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
        fireCountdown = 1f / fireRate;
        GetAllComponents();
    }

    private void Update()
    {
        
        FindTarget();


        if (targetEnemy != null)
        {
            AimAtEnemy();

            // Decrement fireCountdown and shoot if needed
            if (!active)
            {
                return;
            }
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
                missile.SetTarget(targetEnemy.gameObject);
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

    public int GetLevel()
    {
        return level;
    }
    public void Upgrade()
    {
        if (level < 5)
        {
            level++;
            turretDamage = turretDamage * 1.5f;
            range = range*1.25f;
            scoreManager.currency -= upgradeCost;
            currentValue += upgradeCost;
            Debug.Log("Tower upgraded to level " + level);
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
    public void Activate()
    {
        active = true;
    }
    public void Deactivate()
    {
        active = false;
    }
}
