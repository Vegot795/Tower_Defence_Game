using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TSManager : MonoBehaviour
{
    public GameObject previewPrefab;
    public GameObject[] towerPrefabs;
    public TextMeshProUGUI TSInfo;

    private GameObject selectedTowerPrefab;
    private GameObject currentPreview;
    private bool isTowerSelected = false;
    private Vector3 cursorFieldPosition;
    private Vector3 offMapPosition = new Vector3(100, 100, 100);
    void Start()
    {
        HideTSInfo();
        if (towerPrefabs.Length > 0)
        {
            selectedTowerPrefab = null;
        }
    }
    public void CheckCurrentField(Vector3 position)
    {
        this.cursorFieldPosition = position;
    }
    private void FixedUpdate()
    {
        CheckCurrentField(cursorFieldPosition);
    }
    public void SelectTower(int index)
    {
        if (isTowerSelected && selectedTowerPrefab == towerPrefabs[index])
        {
            DeselectTower();
        }
        else
        {
            if (currentPreview != null)
            {
                Destroy(currentPreview);
            }
            if (index >= 0 && index < towerPrefabs.Length) 
            {
                selectedTowerPrefab = towerPrefabs[index];
                isTowerSelected = true;
                Debug.Log("Tower selected:" + selectedTowerPrefab.name);

                ShowPreview();
                ShowTSInfo();
            }
        }
    }
    public void DeselectTower()
    {
        if (isTowerSelected)
        {
            selectedTowerPrefab = null;
            isTowerSelected = false;
            Debug.Log("Tower deselected");

            HidePreview();
            HideTSInfo();
        }
    }

    public GameObject GetSelectedTower() 
    {
        return selectedTowerPrefab; 
    }
    public bool IsTowerSelected()
    {
        return isTowerSelected;
    }

    public GameObject GetPreviewInstance()
    {
        if (currentPreview == null && selectedTowerPrefab != null)
        {
            currentPreview = Instantiate(selectedTowerPrefab);
            SetPreviewMaterialTransparency(currentPreview, 0.5f);
            Collider[] colliders = currentPreview.GetComponentsInChildren<Collider>();

            foreach (var collider in colliders)
            {
                collider.enabled = false;
            }
        }
            Debug.Log("Preview instance created.");
        return currentPreview;
    }
    private void ShowPreview()
    {
        if (currentPreview != null) return;

        if (selectedTowerPrefab != null)
        {
            currentPreview = Instantiate(selectedTowerPrefab);
            Collider previewCollider = currentPreview.GetComponent<Collider>();
            

            if (previewCollider == null)
            {
                previewCollider = currentPreview.GetComponentInChildren<Collider>();
            }
            if (previewCollider != null)
            {
                previewCollider.enabled = false;
            }
            else
            {
                Debug.LogWarning("No collider found");
            }

            currentPreview.SetActive(true);
            SetPreviewMaterialTransparency(currentPreview, 0.5f);
        }
        
    }
    private void HidePreview()
    {
        Destroy(currentPreview);
    }
    private void SetPreviewMaterialTransparency(GameObject preview, float alpha)
    {
        Renderer[] renderers = preview.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                Color color = mat.color;
                color.a = alpha;
                mat.color = color;
            }
        }
    }

    private void ShowTSInfo()
    {
        TSInfo.gameObject.SetActive(true);
    }

    private void HideTSInfo()
    {
        TSInfo.gameObject.SetActive(false);
    }
    
}
