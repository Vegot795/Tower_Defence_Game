using UnityEngine;

public class TurretSelector : MonoBehaviour
{
    public TurretUpgradePanelLogic upgradePanel;
    public GameObject turToPass;
    public GameObject CurrentTurret;

    
    private void Start()
    {
        if (upgradePanel == null)
        {
            Debug.LogError("Assign TurretUpgradePanelLogic in the Inspector.");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current != null &&
                UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                TurretLogic turretLogic = hit.collider.GetComponent<TurretLogic>();
                BturretLogic bTurretLogic = hit.collider.GetComponent<BturretLogic>();



                if (turretLogic != null)
                {
                    GameObject CurrentTurret = turretLogic.gameObject;
                    upgradePanel.Show(turretLogic);
                    upgradePanel.isUpgradePanelActive = true;
                    Debug.Log("Turret was clicked");
                }
                else if (bTurretLogic != null)
                {
                    GameObject CurrentTurret = bTurretLogic.gameObject;
                    upgradePanel.Show(bTurretLogic);
                    upgradePanel.isUpgradePanelActive = true;
                    Debug.Log("Turret was clicked");
                }
            }
            else if (upgradePanel.isUpgradePanelActive)
            {
                upgradePanel.Hide();
                upgradePanel.isUpgradePanelActive = false;
            }
            else
            {
                Debug.Log("No turret clicked or upgrade panel is not active.");
                return;
            }
        }
    }
    public GameObject GetCurrentTurret()
    {
        turToPass = CurrentTurret;
        return turToPass;
    }
}
