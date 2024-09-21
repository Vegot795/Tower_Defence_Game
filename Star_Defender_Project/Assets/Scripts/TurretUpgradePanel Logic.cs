using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class TurretUpgradePanelLogic : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public Button upgradeButton;
    public Button sellButton;

    private GameObject currentTower;
    private GameObject detectedField;

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
        TextMeshProUGUI levelText = GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI upgradeButton = GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI sellButton = GetComponent<TextMeshProUGUI>();
        
        GameObject currentTower = GetComponent<GameObject>();

        ObjectDetection objectDetection = GetComponent<ObjectDetection>();
       

        turretLogic = GetComponent<TurretLogic>();
        bTurretLogic = GetComponent<BturretLogic>();
    }

    void Start()
    {
        gameObject.SetActive(false);
        GetAllComponents();
    }
    private void Update()
    {
        GetDetectedObject(detectedField);

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
        levelText.text = "Level: " + turret.GetLevel();
        transform.position = turret.transform.position + new Vector3(0, 0, 15f);
        gameObject.SetActive(true);
    }
    public void Show(BturretLogic turret)
    {
        currentTower = turret.gameObject;
        levelText.text = "Level: " + turret.GetLevel();
        transform.position = turret.transform.position + new Vector3(0, 0, 15f);
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject?.SetActive(false);
    }
    public void UpgradeTower()
    {
        if (turretLogic != null)
        {
            turretLogic.Upgrade();
            levelText.text = "Level: " + turretLogic.GetLevel();
        }
        else if (bTurretLogic != null)
        {
            bTurretLogic.Upgrade();
            levelText.text = "Level " + bTurretLogic.GetLevel();

        }
    }
    public void SellTower()
    {
        turretLogic.Sell();
    }
}
