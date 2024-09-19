using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerButton : MonoBehaviour
{
    public int towerIndex;
    private TSManager selectionManager;
    // Start is called before the first frame update
    void Start()
    {
        selectionManager = FindAnyObjectByType<TSManager>();
        TextMeshProUGUI button = GetComponent<TextMeshProUGUI>();
        
        
    }

    void OnClick()
    {
        selectionManager.SelectTower(towerIndex);
    }
}
