using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TSManager : MonoBehaviour
{

    public ScoreManager ScoreManager;
    public GameObject BTurretLogic;
    public GameObject TurretLogic;
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
        BTurretLogic = Resources.Load<GameObject>("BTurretLogic");

        TurretLogic = Resources.Load<GameObject>("TurretLogic");

        ScoreManager = FindObjectOfType<ScoreManager>();
    }
    public void CheckCurrentField(Vector3 position)
    {
        this.cursorFieldPosition = position;
    }
    void Update()
    {
        if (isTowerSelected && currentPreview != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                // Snap to grid center
                Vector3 hitPos = hit.collider.transform.position;
                currentPreview.transform.position = hitPos;
                currentPreview.SetActive(true);
            }
            else
            {
                // Hide preview if not over a buildable cube
                //currentPreview.transform.position = offMapPosition;
                currentPreview.SetActive(false);
            }
        }
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
                GameObject towerPrefab = towerPrefabs[index];
                int towerCost = 0;

                // Try to get cost from TurretLogic or BturretLogic
                TurretLogic turretLogic = towerPrefab.GetComponent<TurretLogic>();
                if (turretLogic != null)
                {
                    towerCost = turretLogic.turretCost;
                }
                else
                {
                    BturretLogic bTurretLogic = towerPrefab.GetComponent<BturretLogic>();
                    if (bTurretLogic != null)
                    {
                        towerCost = bTurretLogic.turretCost;
                    }
                }

                // Get player currency (assuming ScoreManager.Instance.Money)
                int playerMoney = ScoreManager.Currency;

                if (playerMoney < towerCost)
                {
                    Debug.Log("Not enough money to select this tower.");
                    
                    DeselectTower();
                    return;
                }

                selectedTowerPrefab = towerPrefab;
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
        //Debug.Log("Preview instance created.");
        return currentPreview;
    }
    private void ShowPreview()
    {
        if (currentPreview != null) return;

        if (selectedTowerPrefab != null)
        {
            currentPreview = Instantiate(selectedTowerPrefab);

            TurretLogic turretLogic = currentPreview.GetComponent<TurretLogic>();
            if (turretLogic != null)
                turretLogic.isInPreview = true;

            BturretLogic bTurretLogic = currentPreview.GetComponent<BturretLogic>();
            if (bTurretLogic != null)
                bTurretLogic.isInPreview = true;
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
        if (currentPreview != null)
        {
            Destroy(currentPreview);
            currentPreview = null;
        }
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