using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelLogic : MonoBehaviour
{
    GameLogic gameLogic;

    public int currentWave;
    public int maxWaves = 20;
    public GameObject WinScreen;
    public TextMeshProUGUI WaveDisplay;

    private int playerHP = 100;
    private int enemiesPerWave = 5;
    private int enemiesToSpawn;
    private int respingTime = 5;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool doesRoundEnded = true;

    private 

    void Start()
    {
        gameLogic = FindObjectOfType<GameLogic>();
        currentWave = 1;
        SpawnEnemies(GetEnemiesToSpawn(enemiesPerWave));

    }


    void Update()
    {
        if (doesRoundEnded && CheckIfAllEnemiesKilled())
        {
            doesRoundEnded = false; // Prevent multiple triggers
            StartNextRound();
        }

        WinTheLevel(currentWave, maxWaves);
        UpdateWaveDisplay();
    }

    

    public int GetEnemiesToSpawn(int enemiesPerWave)
    {
        enemiesToSpawn = enemiesPerWave + (enemiesPerWave * (currentWave-1));
        Debug.Log("Enemies this wave: " + enemiesToSpawn);
        return enemiesToSpawn;
    }
    public void OnEnemyKilled(GameObject enemy)
    {
        spawnedEnemies.Remove(enemy);
    }

    private IEnumerator SpawnEnemiesCoroutine(int enemiesToSpawn)
    {
        int enemyHP = GetEnemyHPForWave(currentWave);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Debug.Log("Spawning enemy " + (i + 1));
            GameObject enemy = gameLogic.SpawnEnemy();
            spawnedEnemies.Add(enemy);

            // Set enemy HP if Enemy script is attached
            EnemyObject enemyScript = enemy.GetComponent<EnemyObject>();
            if (enemyScript != null)
            {
                enemyScript.SetHP(enemyHP);
            }

            int timeBetweenEnemies = enemiesToSpawn / respingTime;
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
    }

    private void SpawnEnemies(int enemiesThisWave)
    {
        StartCoroutine(SpawnEnemiesCoroutine(enemiesThisWave));
    }


    private bool CheckIfAllEnemiesKilled()
    {
        if (spawnedEnemies.Count == 0)
        {
            Debug.Log("All enemies killed. Starting wave ");
            return true;
        }
        return false;
    }

    private void StartNextRound()
    {
        currentWave++;
        int enemiesThisWave = GetEnemiesToSpawn(enemiesPerWave);
        SpawnEnemies(enemiesThisWave);
        doesRoundEnded = true;
    }

    private bool WinTheLevel(int currentWave, int maxWaves)
    {
        if (currentWave == maxWaves && CheckIfAllEnemiesKilled())
        {
            return true;
        }
        else
        {
            return false;
        }   
    }

    private void ShowWinScreen()
    {
        if (currentWave == maxWaves && WinTheLevel(currentWave, maxWaves))
        {
            WinScreen.SetActive(true);
        }
    }

    private void UpdateWaveDisplay()
    {
        if (WaveDisplay != null)
        {
            WaveDisplay.text = $"Wave: {currentWave}/{maxWaves}";
            Debug.Log("WaveDisplay updated: " + WaveDisplay.text);
        }
        else
        {
            Debug.LogWarning("WaveDisplay is not assigned!");
        }
    }
    private int GetEnemyHPForWave(int wave)
    {
        // Example: base HP is 100, increases by 20 per wave
        int baseHP = 20;
        int hpIncreasePerWave = 5;
        return baseHP + (hpIncreasePerWave * (wave - 1));
    }

}

