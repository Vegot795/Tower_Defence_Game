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
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
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
                }
                else if (bTurretLogic != null)
                {
                    // Show upgrade panel for the b-turret
                    upgradePanel.Show(bTurretLogic);
                }
                else
                {
                    // Hide the panel if the clicked object is not a turret
                    upgradePanel.Hide();
                }
            }
            else
            {
                // Hide the panel if nothing is clicked
                upgradePanel.Hide();
            }
        }
    }
}
