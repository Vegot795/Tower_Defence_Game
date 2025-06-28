using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BturretLogic : MonoBehaviour, ILeveler
{

    //Statistics
    public float range = 20f;
    public float fireRate = 1f;
    public float missileSpeed = 20f;
    public float rotationSpeed = 50f;
    public int level = 1;

    private float turretDamage = 10;
    private int cost = 300;
    private int lvlCost = 200;
    //Rest
    public bool isInPreview = false;
    public GameObject missilePrefab;
    public Transform firePoint;
    public Transform targetEnemy;
    public BTAmmoLogic BTAmmoLogic; 

    private float fireCountdown;
    private List<GameObject> EnemiesInRange = new List<GameObject>();
    private int afterSellCurrency;
    private int upgradeCost;
    private int currentValue;
    private int SellMoney;

    ScoreManager scoreManager;
    TurretUpgradePanelLogic upgradePanel;

    public int GetUgpradeCostValue()
    {
        return GetUpgradeCost();
    }

    public int GetSellValue()
    {
        return GetSellMoney();
    }

    private void SetGameLayerRecursive(GameObject _go, int _layer)
    {
        _go.layer = _layer;
        foreach (Transform child in _go.transform)
        {
            child.gameObject.layer = _layer;

            Transform _HasChildren = child.GetComponentInChildren<Transform>();
            if (_HasChildren != null)
                SetGameLayerRecursive(child.gameObject, _layer);

        }
    }

    private void IsInPreview(bool isInPreview)
    {
        this.isInPreview = isInPreview;
        if (isInPreview)
        {
            SetGameLayerRecursive(gameObject, LayerMask.NameToLayer("TurretPreviews"));
            TurretLogic turretLogic = GetComponent<TurretLogic>();
            if (turretLogic != null)
                turretLogic.enabled = false;

            BturretLogic bTurretLogic = GetComponent<BturretLogic>();
            if (bTurretLogic != null)
                bTurretLogic.enabled = false;
        }
        else
        {
            SetGameLayerRecursive(gameObject, LayerMask.NameToLayer("Turrets"));
            TurretLogic turretLogic = GetComponent<TurretLogic>();
            if (turretLogic != null)
                turretLogic.enabled = true;

            BturretLogic bTurretLogic = GetComponent<BturretLogic>();
            if (bTurretLogic != null)
                bTurretLogic.enabled = true;
        }
    }
    
    
    private void GetAllComponents()
    {

        currentValue = cost;
        GetUpgradeCost();
    }
    private void Start()
    {
        Physics.gravity = new Vector3(0, 0, 9.81f); // Set gravity to -Z direction
        fireCountdown = 1f / fireRate;
        GetAllComponents();
        scoreManager = FindObjectOfType<ScoreManager>();

    }
    private void Update()
    {
        IsInPreview(isInPreview);
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

            float h = 5f; // dodatkowy ≥uk wysokoúci
            float gravity = Mathf.Abs(Physics.gravity.z); // 9.81

            // Pozioma odleg≥oúÊ w XY
            Vector3 horizontal = new Vector3(end.x - start.x, end.y - start.y, 0f);
            float horizontalDistance = horizontal.magnitude;

            // PrÍdkoúÊ pionowa (w osi Z) do osiπgniÍcia h
            float vz = Mathf.Sqrt(2 * gravity * h);

            // Czas w gÛrÅEi w dÛ≥
            float timeUp = vz / gravity;
            float heightDifference = start.z - end.z; // UWAGA: start - end, bo Z dzia≥a odwrotnie
            float timeDown = Mathf.Sqrt(2 * Mathf.Max(0f, h + heightDifference) / gravity);
            float totalTime = timeUp + timeDown;

            // PrÍdkoúÊ pozioma
            Vector3 velocityXY = horizontal / totalTime;

            // Pe≥na prÍdkoúÊ
            Vector3 launchVelocity = new Vector3(velocityXY.x, velocityXY.y, -vz); // MINUS, bo -Z to gÛra
            rb.velocity = launchVelocity;

            Debug.Log("Pocisk wystrzelony z prÍdkoúciπ: " + rb.velocity);
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
            int upgradeCost = GetUpgradeCost(); // Store the cost BEFORE changing level
            if (scoreManager.Currency >= upgradeCost)
            {
                level++;
                turretDamage = turretDamage * 1.5f;
                range = range * 1.25f;
                scoreManager.AddCurrency(-upgradeCost);
                currentValue += upgradeCost;
                Debug.Log("Tower upgraded to level " + level);
            }
            else
            {
                Debug.Log("Not enough currency to upgrade. Required: " + upgradeCost + ", Available: " + scoreManager.Currency);
                if (upgradePanel != null)
                    upgradePanel.ShowNotEnoughCredits();
            }
        }
        else
        {
            Debug.Log("Max level reached");
        }
    }

    public void Sell()
    {
        SellMoney = GetSellMoney();
        Debug.Log("Tower Sold For " + SellMoney);
        scoreManager.AddCurrency(SellMoney);
        Destroy(gameObject);
    }
    public int GetUpgradeCost()
    {
        upgradeCost = cost + (lvlCost * level);
        return upgradeCost;
    }
    private int GetCurrentValue()
    {
        currentValue = cost;
        return currentValue;
    }
    public int GetSellMoney()
    {
        afterSellCurrency = currentValue / 3;
        return afterSellCurrency;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
