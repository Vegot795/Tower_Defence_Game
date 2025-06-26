using UnityEngine;

public class TurretSelector : MonoBehaviour
{

    public TurretUpgradePanelLogic upgradePanel;

    private void Start()
    {
        upgradePanel = GetComponent<TurretUpgradePanelLogic>();
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
                // Check if the clicked object is a turret
                TurretLogic turretLogic = hit.collider.GetComponent<TurretLogic>();
                BturretLogic bTurretLogic = hit.collider.GetComponent<BturretLogic>();

                if (turretLogic != null)
                {
                    // Show upgrade panel for the turret
                    upgradePanel.Show(turretLogic);
                    upgradePanel.isUpgradePanelActive = true;

                }
                else if (bTurretLogic != null)
                {
                    // Show upgrade panel for the b-turret
                    upgradePanel.Show(bTurretLogic);
                    upgradePanel.isUpgradePanelActive = true;
                }                
            }
            else if (upgradePanel.isUpgradePanelActive)
            {
                // Hide the panel if nothing is clicked
                upgradePanel.Hide();
                upgradePanel.isUpgradePanelActive = false;
            }
            else
            {
                return;
            }
        }
    }
}
