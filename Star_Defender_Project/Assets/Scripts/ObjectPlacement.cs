using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    private TSManager selectionManager;
    private ObjectDetection detector;
    private GameObject currentField;

    private void Start()
    {

        selectionManager = FindAnyObjectByType<TSManager>();
        detector = FindAnyObjectByType<ObjectDetection>();

        if (selectionManager == null)
        {
            Debug.LogError("SelectionManager not found! Make sure it's in the scene.");
        }

        if (detector == null)
        {
            Debug.LogError("ObjectDetection not found! Make sure it's in the scene.");
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
            //Debug.Log("Selected tile:" + currentField.name);
        }
    }
    private void Update()
    {
        Debug.Log("Update method running...");
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
        Debug.Log("Attempting to place tower...");

        if (currentField == null)
        {
            Debug.Log("Current field is null");
        }
        if (currentField != null)
        {
            GameObject turretPrefab = selectionManager.GetSelectedTower();
            if (turretPrefab == null)
            {
                Debug.Log("No tower prefab selected");
                return;
            }

            if (currentField.CompareTag("MapTile"))
            {
                Vector3 position = new Vector3(currentField.transform.position.x, currentField.transform.position.y, currentField.transform.position.z - 0.4f);
                Instantiate(turretPrefab, position, Quaternion.identity);
                Debug.Log("Tower placed at:" + position);
                currentField = null;
                selectionManager.DeselectTower();

            }
            else
            {
                Debug.Log($"{currentField.name} is not available spot");
            }

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
