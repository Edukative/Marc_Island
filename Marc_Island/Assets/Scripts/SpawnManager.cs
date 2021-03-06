﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    float spawnRange = 10;
    public float minMassRange = 0.5f;
    public float maxMassRange = 2.5f;
    public GameObject player;
    bool isGameOver = false;
    public Text gameOverText;
    GameObject canvas;
    Vector3 initialPosition;

    int waves = 1;
    public GameObject powerUpPrefab;


    // Start is called before the first frame update
    void Start()
    {

        SpawnEnemyWave(waves);
        // something to make spawn
        Instantiate(powerUpPrefab, GenerateRandomPosition(), powerUpPrefab.transform.rotation);

        player = GameObject.Find("Player");
        initialPosition = player.transform.position;
        canvas = GameObject.Find("Canvas");
        gameOverText = canvas.transform.GetChild(0).GetComponent<Text>();
        gameOverText.enabled = false; // set unactive the text
    }

    // Update is called once per frame
    void Update()
    {
        int enemiesCount = FindObjectsOfType<EnemyMovement>().Length;
        // find all the gameObjects in the scene with the component EnemyScript
        if (enemiesCount == 0 && !isGameOver) // there are no more enemies
        {
            waves++;
            SpawnEnemyWave(waves);
            Instantiate(powerUpPrefab, GenerateRandomPosition(), powerUpPrefab.transform.rotation);
        }
        if (player.transform.position.y <= -10) // the player falls from the island
        {
            isGameOver = true;
            gameOverText.enabled = true;
            if (Input.anyKeyDown) // the player has dissapeared
            {
                RestartGame();
            }
        }
    }

    Vector3 GenerateRandomPosition()
    {
        float spawnRandomX = Random.Range(-spawnRange, spawnRange);
        float spawnRandomZ = Random.Range(-spawnRange, spawnRange);
        Vector3 spawnRandomPosition = new Vector3(spawnRandomX, 0, spawnRandomZ);
        Debug.Log("Random position" + spawnRandomPosition);
        return spawnRandomPosition;
    }

    void RestartGame ()
    {
        player.transform.position = initialPosition;
        waves = 1;
        SpawnEnemyWave(waves);
        isGameOver = false;
        gameOverText.enabled = false;

        int powerupCount = GameObject.FindGameObjectsWithTag("Powerup").Length; //get the number if powerups
        if (powerupCount == 0) // there is no powerup in the scene
        {
            Instantiate(powerUpPrefab, GenerateRandomPosition(), powerUpPrefab.transform.rotation); // spawns 1
        }
        else if (powerupCount > 1) // more than 1 powerup in the island
        {
            for (int i = 0; i < powerupCount; i++)  // in all the powerups except 1...
            {
                GameObject PowerupToDestroy = GameObject.FindGameObjectWithTag("Powerup"); // find 1 gameObject with this tag...
                Destroy(PowerupToDestroy); // and destroy it
            }
        }
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, GenerateRandomPosition(), enemyPrefab.transform.rotation);
            Rigidbody enemyRB = enemy.GetComponent<Rigidbody>();
            EnemyMovement enemyScript = enemy.GetComponent<EnemyMovement>();

            enemyRB.mass = Random.Range(minMassRange, maxMassRange);
            float currentMass = enemyRB.mass;
            enemy.transform.localScale = new Vector3(currentMass, currentMass, currentMass);
            // current scale is changed according to the randomized mass of the enemy spawned
            enemyScript.speed = enemyScript.speed + currentMass;
        }
    }
}
