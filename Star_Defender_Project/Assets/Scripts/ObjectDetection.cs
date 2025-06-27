using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class ObjectDetection : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask interactableLayer;
    public TurretUpgradePanelLogic upgradePanelLogic; // Assign in Inspector

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        DetectObjectUnderCursor();

        // Handle turret click for upgrade panel
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
            {
                // Try TurretLogic
                TurretLogic turret = hit.collider.GetComponent<TurretLogic>();
                if (turret != null)
                {
                    upgradePanelLogic.Show(turret);
                    return;
                }
                // Try BturretLogic
                BturretLogic bTurret = hit.collider.GetComponent<BturretLogic>();
                if (bTurret != null)
                {
                    upgradePanelLogic.Show(bTurret);
                    return;
                }
            }
        }
    }

    void DetectObjectUnderCursor()
    {
        if (mainCamera != null)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 1.0f);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
            {
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.CompareTag("MapTile"))
                {
                    //ray = mainCamera.ScreenPointToRay(hitObject.transform.position);
                    ///f(Physics.Raycast())
                    //if()
                    FindObjectOfType<ObjectPlacement>().SetSelectedCube(hitObject);
                }
                else if (hitObject.CompareTag("Turret"))
                {
                    FindObjectOfType<ObjectPlacement>().SetSelectedTurret(hitObject);
                    Debug.Log("Detected new Turret: " + hitObject.name);
                }
                else
                {
                    FindObjectOfType<ObjectPlacement>().SetSelectedCube(null);
                }
            }
        }
        else
        {
            Debug.LogError("Main camera not found");
        }
    }
}
