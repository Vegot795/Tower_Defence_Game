using UnityEngine;
using UnityEngine.EventSystems;

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
            // If pointer is over any UI, do nothing (don't select/hide)
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
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
                    CurrentTurret = turretLogic.gameObject;
                    upgradePanel.Show(turretLogic);
                    upgradePanel.isUpgradePanelActive = true;
                    Debug.Log("Turret was clicked");
                    return;
                }
                else if (bTurretLogic != null)
                {
                    CurrentTurret = bTurretLogic.gameObject;
                    upgradePanel.Show(bTurretLogic);
                    upgradePanel.isUpgradePanelActive = true;
                    Debug.Log("Turret was clicked");
                    return;
                }
            }

            // If we get here, no turret was clicked and not over UI, so hide the panel
            upgradePanel.Hide();
            upgradePanel.isUpgradePanelActive = false;
            Debug.Log("No turret clicked, hiding upgrade panel.");
        }
    }

    public GameObject GetCurrentTurret()
    {
        turToPass = CurrentTurret;
        return turToPass;
    }
}
