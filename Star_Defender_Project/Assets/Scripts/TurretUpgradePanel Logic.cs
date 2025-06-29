using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEditor.Overlays;

public class TurretUpgradePanelLogic : MonoBehaviour
{
    GameObject currentTower;
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI sellText;
    public GameObject UpgradePanel;
    public TextMeshProUGUI lvlDisplay;
    public TextMeshProUGUI NotEnoughCredits;

    public bool isUpgradePanelActive = false;

    private GameObject detectedField;
    private GameObject spawnedPanel;

    TurretLogic turretLogic;
    BturretLogic bTurretLogic;
    ObjectDetection objectDetection;
    private void GetAllComponents()
    {
        TextMeshProUGUI UpgradeBut = GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI SellBut = GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI NotEnoughCredits = GetComponent<TextMeshProUGUI>();

        GameObject currentTower = GetComponent<GameObject>();

        objectDetection = GetComponent<ObjectDetection>();


        turretLogic = GetComponent<TurretLogic>();
        bTurretLogic = GetComponent<BturretLogic>();
    }

    public void SetTarget(GameObject CurrentTower)
    {
        this.currentTower = CurrentTower;
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
        turretLogic = turret;
        bTurretLogic = null;
        currentTower = turret.gameObject;
        Debug.Log($"TurretLogic: {turretLogic != null}, BturretLogic: {bTurretLogic != null}");

        Vector3 positionToDisplay = turret.transform.position;
        UpgradePanel.transform.position = positionToDisplay;
        isUpgradePanelActive = true;
        UpgradePanel.SetActive(true);

        upgradeText.text = $"Upgrade: ({turret.GetUgpradeCostValue()}$)";
        sellText.text = $"Sell: ({turret.GetSellValue()}$)";
        lvlDisplay.text = $"Level: {turret.GetLevel()}";
    }
    public void Show(BturretLogic turret)
    {
        turretLogic = null;
        bTurretLogic = turret;
        currentTower = turret.gameObject;
        Debug.Log($"TurretLogic: {turretLogic != null}, BturretLogic: {bTurretLogic != null}");

        Vector3 positionToDisplay = currentTower.transform.position - new Vector3(0, 0, 1);
        UpgradePanel.transform.position = positionToDisplay;
        isUpgradePanelActive = true;
        UpgradePanel.SetActive(true);        

        upgradeText.text = $"Upgrade: ({turret.GetUgpradeCostValue()}$)";
        sellText.text = $"Sell: ({turret.GetSellValue()}$)";
        lvlDisplay.text = $"Level: {turret.GetLevel()}";
    }
    public void Hide()
    {
        if (UpgradePanel != null)
        {
            UpgradePanel.SetActive(false);
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
        Hide();
    }
    public void SellTower()
    {
        Debug.Log("SellTower called. turretLogic: " + (turretLogic != null) + ", bTurretLogic: " + (bTurretLogic != null));
        if (turretLogic != null)
        {
            turretLogic.Sell();
        }
        else if (bTurretLogic != null)
        {
            bTurretLogic.Sell();
        }
        Hide();
    }
    public void ShowNotEnoughCredits()
    {

        NotEnoughCredits.text = "Not enough credits!";

    }

    private IEnumerator FadeOutNotEnoughCredits()
    {
        yield return new WaitForSeconds(2f);
        NotEnoughCredits.gameObject.SetActive(false);
    }
    
}
