using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonHandlerMapGen : MonoBehaviour
{
    
    public bool IsRBMenuOn = false;
    public Button ToggleButton;
    //public Image buttonImage;
    public Color activeColor = Color.green;
    public Color inactiveColor = Color.red;
    public Material ToggleMaterial;

    // Start is called before the first frame update
    void Start()
    {
        ToggleMaterial.color = inactiveColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToggleRBMenu()
    {
        IsRBMenuOn = !IsRBMenuOn;

        if (IsRBMenuOn)
        {
            //Debug.Log("Road Builder is on");
            ToggleMaterial.color = activeColor;
        }
        else
        {
            //Debug.Log("Road Builder is off");
            ToggleMaterial.color = inactiveColor;
        }
        //Debug.Log("Button Clicked");
    }
}
