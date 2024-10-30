using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoadBuilder : MonoBehaviour
{
    public LayerMask interactableLayer;
    public Camera mainCamera;
    public GameObject RoadTile;
    public GameObject BorderTile;
    public GameObject RoadTilesHolder;
    public GameObject BorderTilesHolder;
    public ButtonHandlerMapGen butHand;
    public GameObject StartPointPrefab;
    public GameObject EndPointPrefab;
    public int MinimapRoadLength = 2;


    private GameObject hoveredField;
    private ObjectDetection detector;
    private MapGeneragot mapGen;
    private List<GameObject> _BTList = new List<GameObject>();
    private List<GameObject> RoadList = new List<GameObject>();
    private GameObject StartPoint = null;
    private GameObject EndPoint = null;
    private GameObject currentField = null;



    // Start is called before the first frame update
    void Start()
    {
        mapGen = FindAnyObjectByType<MapGeneragot>();
        BorderInstantiator();

        if (butHand != null)
        {
            Debug.Log("butHand Successfully Assigned");
        }
    }


    // Update is called once per frame
    void Update()
    {
        CheckHoveredCube();
        if (butHand != null && butHand.IsRBMenuOn)
        {
            CheckForRoadBuildingInput();
        }
    }

    private void CheckHoveredCube()
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
                    hoveredField = hitObject;
                    //Debug.Log("Current Hovered Field:" + hoveredField);
                }
            }
            else
            {
                //Debug.Log("No Hovered Field");
            }
        }
        else
        {
            hoveredField = null;
            //Debug.Log("No Hovered Field");

        }
    }
    public void BorderInstantiator()
    {
        _BTList = mapGen.BTList;

        if (_BTList.Count < 1)
        {
            Debug.Log("BTList is empty");
            return;
        }

        foreach (var newTile in _BTList)
        {
            GameObject InstBordTile = Instantiate(BorderTile, newTile.transform.position, Quaternion.identity);
            InstBordTile.transform.SetParent(BorderTilesHolder.transform);
        }
    }

    private void BuildRoad()
    {
        GameObject newRoadTile = Instantiate(RoadTile, hoveredField.transform.position, Quaternion.identity);
        newRoadTile.transform.SetParent(RoadTilesHolder.transform);
        RoadList.Add(newRoadTile);

    }

    private void CheckForRoadBuildingInput() // this makes us build road on left mouse click
    {
        if (hoveredField != null && hoveredField.CompareTag("MapTile") && Input.GetMouseButtonDown(0)) // Check if cursor is above map tile & Detects if player want to place road
        {
            bool isBorderTile = _BTList.Contains(hoveredField);

            if (StartPoint == null && isBorderTile) // If this will be first placed road tile, it has to be on the border of the map to create Start Point
            {
                BuildRoad();
                StartPoint = Instantiate(StartPointPrefab, hoveredField.transform.position, Quaternion.identity);
                Debug.Log("Phase I - Start Point Assigned To: " + StartPoint.transform.position);
            }
            else if (StartPoint != null && RoadList.Count < MinimapRoadLength) // Checks if road list has enough tiles to end, if not, player has to build in center of the map
            {
                if (!isBorderTile) // checks if hovered field is not the border field
                {
                    BuildRoad();
                    Debug.Log($"Phase II - Map Tiles {RoadList.Count} out of {MinimapRoadLength}");
                }
                else
                {
                    Debug.Log("Cannot end building your road so fast");
                }
            }
            else if (StartPoint != null && RoadList.Count >= MinimapRoadLength) // Checks if road is long enough;
            {
                BuildRoad();
                Debug.Log("Road built on:" + hoveredField.name + hoveredField.transform.position);
                Debug.Log("Phase III");

                if (isBorderTile) // If player builds road on the border above min road lenght, end point will be instantiated
                {
                    GameObject EndPoint = Instantiate(EndPointPrefab, hoveredField.transform.position, Quaternion.identity);
                    butHand.IsRBMenuOn = false;
                    Debug.Log("Phase IV - End Point Assigned To: " + EndPoint.transform.position);
                }

            }
            else
            {
                Debug.Log("Cannot build road here: " + hoveredField.transform.position);
                if (StartPoint != null)
                {
                    Debug.Log("Start Point: " + StartPoint.transform.position);
                }
                Debug.Log("Border Tile: " + isBorderTile);
                Debug.Log(" Hovered Field: " + hoveredField.transform.position);
                //Debug.Log($"Start Point: {StartPoint.transform.position} | Border Tile: {isBorderTile} |");
            }
        }
    }
    private void CheckForNeighbours()
    {
        if (StartPoint != null)
        {
            if (currentField == null)
            {
                currentField.transform.position = StartPoint.transform.position;
            }


        }
        else
        {
            return;
        }
    }
    private bool IsOccupied() {

    }
    private void OnTriggerStay(Collider other) {
        
    }
    

}
