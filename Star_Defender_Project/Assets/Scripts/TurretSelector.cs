using UnityEngine;

public class TurretSelector : MonoBehaviour
{
    public TurretUpgradePanelLogic upgradePanel;

    private void Start()
    {
       if (upgradePanel == null)
        {
            Debug.Log("Upgrade panel is null");
        }
    }
    private void Update()
    {
        Debug.Log("TurretSelector is running");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0)) 
        {
            Debug.Log("Mouse Clicked");

            if (Physics.Raycast(ray, out hit))
            {
                TurretLogic turretLogic = hit.collider.GetComponent<TurretLogic>();
                BturretLogic bTurretLogic = hit.collider.GetComponent<BturretLogic>();

                if (turretLogic != null)
                {
                    upgradePanel.Show(turretLogic);
                }
                else if (bTurretLogic != null)
                {
                    upgradePanel.Show(bTurretLogic);
                }
                else
                {
                    upgradePanel.Hide();
                }
            }
            else
            {                
                upgradePanel.Hide();
            }
        }
        else
        {

        }
    }
}
