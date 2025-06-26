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
    public float tileDistance = 1f;
    public List<GameObject> RoadList = new List<GameObject>();


    private GameObject hoveredField;
    private ObjectDetection detector;
    private MapGeneragot mapGen;
    public List<GameObject> _BTList = new List<GameObject>();
    private GameObject StartPoint = null;
    private GameObject currentField;



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
        currentField = hoveredField;
    }

    private void CheckForRoadBuildingInput() // this makes us build road on left mouse click
    {
        if (hoveredField != null && hoveredField.CompareTag("MapTile") && Input.GetMouseButtonDown(0)) // Check if cursor is above map tile & Detects if player want to place road
        {
            bool isBorderTile = _BTList.Contains(hoveredField); // bool if object is border tile or not
            if (StartPoint == null && isBorderTile) // If this will be first placed road tile, it has to be on the border of the map to create Start Point
            {
                BuildRoad();
                StartPoint = Instantiate(StartPointPrefab, hoveredField.transform.position, Quaternion.identity);

                Debug.Log("Phase I - Start Point Assigned To: " + StartPoint.transform.position);
            }
            else if (StartPoint != null)
            {
                List<GameObject> neigList = GetNeighbours(currentField);
                bool isNeighbor = neigList.Contains(hoveredField); // checks if next tile is a neighbor of the currentField

                if (isNeighbor && RoadList.Count < MinimapRoadLength) // Checks if road list has enough tiles to end, if not, player has to build in center of the map
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
                else if (isNeighbor && RoadList.Count >= MinimapRoadLength) // Checks if road is long enough;
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
                /*else //  END | Message that something is messed up
                {
                    Debug.Log("Cannot build road here: " + hoveredField.transform.position);
                    if (StartPoint != null)
                    {
                        Debug.Log("Start Point: " + StartPoint.transform.position);
                    }
                    Debug.Log("Border Tile: " + isBorderTile);
                    Debug.Log(" Hovered Field: " + hoveredField.transform.position);
                }*/
            }

            else
            {
                Debug.Log("You try to build the road on the field other than neighboring");
            }
        }
    }

    private List<GameObject> GetNeighbours(GameObject field)
    {
        List<GameObject> neighbours = new List<GameObject>();

        Vector3 fieldPosition = field.transform.position;

        Vector3[] directions = new Vector3[] // this is responsible for directions in looking
        {
            new Vector3(tileDistance, 0, 0),
            new Vector3(-tileDistance, 0, 0),
            new Vector3(0, tileDistance, 0),
            new Vector3(0, -tileDistance, 0)
        };

        foreach (var direction in directions)
        {
            Vector3 neighbourPosition = fieldPosition + direction;

            Collider[] hitColliders = Physics.OverlapSphere(neighbourPosition, 0.1f, interactableLayer);
            foreach (Collider hitCollider in hitColliders)
            {
                GameObject neighbor = hitCollider.gameObject;
                if (neighbor.CompareTag("MapTile"))
                {
                    neighbours.Add(neighbor);
                    Debug.Log("Found neighbor: " + neighbor.transform.position);
                }
            }
        }
        return neighbours;
    }
}
