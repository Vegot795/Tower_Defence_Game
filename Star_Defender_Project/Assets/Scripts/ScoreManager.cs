using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreLabel;
    public TextMeshProUGUI CurrenctyLabel;
    public GameObject UpgradePanel;

    public int StartCurrency = 150;
    public int score = 0;
    public int currency = 0;

    private void GetAllComponents()
    {
        TextMeshProUGUI ScoreLabel = GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI CurrencryLabel = GetComponent<TextMeshProUGUI>();
        GameObject UpgradePanel = GetComponent<GameObject>();
    }
    public void Start()
    {
        currency += StartCurrency;

        GetAllComponents();
        ShowStats();
    }

    public int Score
    {
        get { return score;  }
        private set
        {
            score = value;
            //Debug.Log("Score updated:" + score);
            ShowStats();
        }
    }
    public void AddScore(int scoreAmount)
    {
        Score += scoreAmount;
    }
    public int Currency
    {
        get { return currency; }
        private set
        {
            currency = value;
            //Debug.Log("Currency updated: " + currency);
            ShowStats();
        }
    }
    public void AddCurrency(int currencyAmount)
    {
        Currency += currencyAmount;
    }
    public void ShowStats()
    {
        if (ScoreLabel != null)
        {
            ScoreLabel.text = "Score: " + score;
            
        }
        else
        {
            Debug.LogWarning("Score label not assigned in the inspector");
        }
        if (CurrenctyLabel != null)
        {
            CurrenctyLabel.text = "Currency: " + currency;
        }
        else
        {
            Debug.LogWarning("Currency label not assigned in the inspector");
        }
    }

}
