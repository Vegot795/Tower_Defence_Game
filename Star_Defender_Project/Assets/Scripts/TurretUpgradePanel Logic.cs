using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEditor.Overlays;

public class TurretUpgradePanelLogic : MonoBehaviour
{
    public GameObject UpgradePanel;
   
    private GameObject currentTower;
    private GameObject detectedField;
    public bool isUpgradePanelActive = false;

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

        ObjectDetection objectDetection = GetComponent<ObjectDetection>();
       

        turretLogic = GetComponent<TurretLogic>();
        bTurretLogic = GetComponent<BturretLogic>();
    }

    private void UpgradePaneController(bool isUpgradePanelActive)
    {
        if (isUpgradePanelActive)
        {
            UpgradePanel.SetActive(true);
        }
        else
        {
            UpgradePanel.SetActive(false);
        }
    }   

    void Start()
    {
   
        isUpgradePanelActive = false;
        GetAllComponents();
        
    }
    private void Update()
    {
        GetDetectedObject(detectedField);
        UpgradePaneController(isUpgradePanelActive);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
        }
        if (Input.GetMouseButton(0) && !objectDetection.CompareTag("UpgradePanel"))
        {
            Hide();
        }
    }


    public void Show(TurretLogic turret)
    {
        currentTower = turret.gameObject;
        Vector3 positionToDisplay = turret.transform.position;
        Instantiate(UpgradePanel, positionToDisplay, Quaternion.identity);
        isUpgradePanelActive = true;

    }
    public void Show(BturretLogic turret)
    {
        currentTower = turret.gameObject;
        Vector3 positionToDisplay = turret.transform.position;
        Instantiate(UpgradePanel, positionToDisplay, Quaternion.identity);
        isUpgradePanelActive = true;
    }
    public void Hide()
    {
        isUpgradePanelActive = false;
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
