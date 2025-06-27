using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    private TSManager selectionManager;
    private ObjectDetection detector;
    private GameObject currentField;
    private ScoreManager scoreManager;
    

    private void Start()
    {
        selectionManager = FindAnyObjectByType<TSManager>();
        detector = FindAnyObjectByType<ObjectDetection>();
        scoreManager = FindAnyObjectByType<ScoreManager>(); // Initialize ScoreManager

        if (selectionManager == null)
        {
            Debug.LogError("SelectionManager not found! Make sure it's in the scene.");
        }

        if (detector == null)
        {
            Debug.LogError("ObjectDetection not found! Make sure it's in the scene.");
        }

        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager not found! Make sure it's in the scene.");
        }
    }

    public void SetSelectedCube(GameObject hitObject)
    {
        this.currentField = hitObject;
        if (currentField == null)
        {
            Debug.Log("HitObject not found");
        }
        else
        {
            Debug.Log($"SetSelectedCube: {currentField.name}, tag: {currentField.tag}");
        }
    }

    private void Update()
    {
        if (selectionManager.IsTowerSelected())
        {
            UpdatePreviewPosition();
            if (Input.GetMouseButtonDown(0))
            {
                PlaceTower();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancelTowerPlacement();
            }
        }
    }

    void PlaceTower()
    {
        if (currentField == null)
        {
            Debug.Log("Current field is null");
            return;
        }

        GameObject turretPrefab = selectionManager.GetSelectedTower();
        if (turretPrefab == null)
        {
            Debug.Log("No tower prefab selected");
            return;
        }

        int cost = 0;
        TurretLogic turretLogic = turretPrefab.GetComponent<TurretLogic>();
        if (turretLogic != null)
        {
            cost = turretLogic.turretCost;
        }
        else
        {
            BturretLogic bTurretLogic = turretPrefab.GetComponent<BturretLogic>();
            if (bTurretLogic != null)
            {
                cost = bTurretLogic.turretCost;
            }
        }

        if (scoreManager != null && cost > 0)
        {
            if (scoreManager.currency < cost)
            {
                Debug.Log("Not enough credits to place tower.");
                // Optionally show UI feedback here
                return;
            }
        }

        if (currentField.CompareTag("MapTile"))
        {
            Vector3 position = new Vector3(currentField.transform.position.x, currentField.transform.position.y, currentField.transform.position.z - 0.4f);
            GameObject turret = Instantiate(turretPrefab, position, Quaternion.identity);

            // Deduct cost from currency pool
            if (scoreManager != null && cost > 0)
            {
                scoreManager.RemoveCurrency(cost);
            }

            // Set isInPreview on the instance, not the prefab
            TurretLogic instanceTurretLogic = turret.GetComponent<TurretLogic>();
            if (instanceTurretLogic != null)
            {
                instanceTurretLogic.isInPreview = true;
            }
            else
            {
                BturretLogic instanceBTurretLogic = turret.GetComponent<BturretLogic>();
                if (instanceBTurretLogic != null)
                {
                    instanceBTurretLogic.isInPreview = true;
                }
            }

            Collider previewCollider = turret.GetComponent<Collider>();

            currentField = null;
            selectionManager.DeselectTower();
        }
        else
        {
            Debug.Log($"{currentField.name} is not available spot");
        }
    }

    void CancelTowerPlacement()
    {
        selectionManager.DeselectTower();
        Debug.Log("Turret placement cancelled");
    }

    void UpdatePreviewPosition()
    {
        if (selectionManager.IsTowerSelected() && currentField != null)
        {
            GameObject previewInstance = selectionManager.GetPreviewInstance();
            if (previewInstance != null)
            {
                Vector3 position = new Vector3(currentField.transform.position.x, currentField.transform.position.y, currentField.transform.position.z - 0.4f);
                previewInstance.transform.position = position;
            }
        }
    }
}
