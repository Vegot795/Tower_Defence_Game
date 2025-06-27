using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelLogic : MonoBehaviour
{
    GameLogic gameLogic;


    public int currentWave;
    public int maxWaves = 20;
    public int playerHP;

    public GameObject WinScreen;
    public GameObject DeathScreen;
    public TextMeshProUGUI WaveDisplay;
    public TextMeshProUGUI PlayerHPDisplay;

    private int enemiesPerWave = 5;
    private int enemiesToSpawn;
    private int respingTime = 5;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool doesRoundEnded = true;
    private int respawnTime;

    private 

    void Start()
    {
        gameLogic = FindObjectOfType<GameLogic>();
        currentWave = 1;
        SpawnEnemies(GetEnemiesToSpawn(enemiesPerWave));
        playerHP = 25;

    }


    void Update()
    {
        if (doesRoundEnded && CheckIfAllEnemiesKilled())
        {
            if(currentWave <= maxWaves)
            {
                Debug.Log("Max waves reached, no more rounds will start.");
                doesRoundEnded = false; // Prevent multiple triggers
                StartNextRound();
            }
            else
            {
                return;
            }
        }

        WinTheLevel(currentWave, maxWaves);
        if (WinTheLevel(currentWave, maxWaves))
        {
            ShowWinScreen();
        }

        UpdateWaveDisplay();
        TrackPlayerHP();
        if (playerHP <= 0 && !DeathScreen.activeSelf)
        {
            ShowDeathScreen();
            Time.timeScale = 0;
        }
    }


    public int GetEnemiesToSpawn(int enemiesPerWave)
    {
        enemiesToSpawn = enemiesPerWave + (int)(enemiesPerWave * ((currentWave - 1) / 2f));
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
        int respawnTime = GetRespawnTime();

        float timeBetweenEnemies = (float)respawnTime / enemiesToSpawn;

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
        if(currentWave < maxWaves)
        {
            currentWave++;
            int enemiesThisWave = GetEnemiesToSpawn(enemiesPerWave);
            SpawnEnemies(enemiesThisWave);
            doesRoundEnded = true;
           
        }
        else
        {
            Debug.Log("No more rounds to start. Max waves reached.");
            ShowWinScreen();
        }
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

    public void EnemyTouchedTarget()
    {
        playerHP--;
    }

    private int TrackPlayerHP()
    {
        PlayerHPDisplay.text = $"Player HP: {playerHP}";
        return playerHP;
    }

    private int GetRespawnTime()
    {
        respawnTime = respingTime + (1*currentWave -1);
        return respawnTime;
    }
    private void ShowDeathScreen()
    {
        DeathScreen.SetActive(true);
    }
}

