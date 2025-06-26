using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEditor.Overlays;

public class TurretUpgradePanelLogic : MonoBehaviour
{
    public GameObject UpgradePanel;
    public bool isUpgradePanelActive = false;

    private GameObject currentTower;
    private GameObject detectedField;
    private GameObject spawnedPanel;

    TurretLogic turretLogic;
    BturretLogic bTurretLogic;
    ObjectDetection objectDetection;

    // Start is called before the first frame update
    private void GetDetectedObject(GameObject currentField)
    {
        this.detectedField = currentField.gameObject;
    }
    private void GetAllComponents()
    {
        TextMeshProUGUI UpgradeButton = GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI SellButton = GetComponent<TextMeshProUGUI>();
        
        GameObject currentTower = GetComponent<GameObject>();

        objectDetection = GetComponent<ObjectDetection>();
       

        turretLogic = GetComponent<TurretLogic>();
        bTurretLogic = GetComponent<BturretLogic>();
    }

    void Start()
    {
   
        UpgradePanel.SetActive(false);
        GetAllComponents();
        
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
        }
    }


    public void Show(TurretLogic turret)
    {
        if (spawnedPanel != null)
        {
            Hide();
            return;
        }
        currentTower = turret.gameObject;
        Vector3 positionToDisplay = turret.transform.position;
        spawnedPanel = Instantiate(UpgradePanel, positionToDisplay, Quaternion.identity);
        Debug.Log("Upgrade panel spawned at: " + positionToDisplay);
        isUpgradePanelActive = true;
        spawnedPanel.SetActive(true);

    }
    public void Show(BturretLogic turret)
    {
        if (spawnedPanel != null)
        {
            Hide();
            return;
        }
        currentTower = turret.gameObject;
        Vector3 positionToDisplay = turret.transform.position;
        spawnedPanel = Instantiate(UpgradePanel, positionToDisplay, Quaternion.identity);
        Debug.Log("Upgrade panel spawned at: " + positionToDisplay);
        isUpgradePanelActive = true;
        spawnedPanel.SetActive(true);
    }
    public void Hide()
    {
        if (spawnedPanel != null)
        {
            spawnedPanel.SetActive(false);
            Destroy(spawnedPanel);
            Debug.Log("Upgrade panel destroyed.");
            spawnedPanel = null;
            isUpgradePanelActive = false;

        }
    }
    public void UpgradeTower()
    {
        if (turretLogic != null)
        {
            turretLogic.Upgrade();
        }
        else if (bTurretLogic != null)
        {
            bTurretLogic.Upgrade();

        }
    }
    public void SellTower()
    {
        turretLogic.Sell();
    }
}
