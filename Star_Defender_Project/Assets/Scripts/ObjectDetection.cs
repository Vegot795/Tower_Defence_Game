using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class ObjectDetection : MonoBehaviour
{
    public Camera mainCamera; // Assign the main camera if not using Camera.main
    public LayerMask interactableLayer; // Set this to the layer of objects you want to interact with

    private void Start()
    {
        mainCamera = Camera.main;
        
    }
    void Update()
    {
        DetectObjectUnderCursor();
    }

    void DetectObjectUnderCursor()
    {
        if (mainCamera != null )
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
                    Debug.Log("Detected new Tile: " + hitObject.name);
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
            else
            {
                //Debug.Log("Cursor is not hovering over any interactable object.");
                
            }
        }
        else
        {
            Debug.LogError("Main camera not found");
        }
    }
}
