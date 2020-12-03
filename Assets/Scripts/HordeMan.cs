using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeMan : MonoBehaviour {

    public int waveCount = 5;
    public int increment = 1;
    public int timeBetween = 600;
    public Vector2[] spawnPoints;
    private float currentTime = 120;
    public float minimumSpace = 10;
    public float checkForSpawnTime = 5;
    private float currentSpawnTime = 0;
    public GameObject basicEnemyPrefab;
    private PlayerScript playerHandler;

    void Start() {
        playerHandler = GameObject.FindObjectOfType<PlayerScript>();
    }

    void Update() {
        CheckWave();
    }

    void CheckWave() {
        if (currentTime < 0) {
            if (currentSpawnTime < 0) {
                currentSpawnTime = checkForSpawnTime;
                if (WaveCreate()) {
                    currentTime = timeBetween;
                    Debug.Log("Wave spawn finished");
                } else Debug.Log("No room to spawn");
            } else {
                currentSpawnTime -= Time.deltaTime;
            }
        } else {
            currentTime -= Time.deltaTime;
        }
    }

    bool CheckSpace(GameObject[] objects, Vector2 position, float radius) {
        foreach (GameObject obj in objects) {
            if ((position - (Vector2)obj.transform.position).magnitude < radius) {
                Debug.Log("Spawn space not OK");
                return false;
            }
        }
        Debug.Log("Spawn space OK");
        return true;
    }

    bool WaveCreate() {
        int enemyCount = waveCount;

        Vector2 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject[] allGM = GameObject.FindGameObjectsWithTag("Ship");

        if (!CheckSpace(allGM, spawnPoint, (Mathf.Sqrt(enemyCount) + 1) * minimumSpace)) return false;

        int row = (int)Mathf.Sqrt(waveCount);
        int rowCounter = 0;
        int columnCounter = 0;

        Vector2 offset = new Vector2(row / 2f, enemyCount / row / 2f);

        while (enemyCount-- > 0) {
            if (rowCounter == row) {
                rowCounter = 0;
                columnCounter++;
            }

            GameObject newEnemy = GameObject.Instantiate(basicEnemyPrefab);
            Rigidbody2D rbHanler = newEnemy.GetComponent<Rigidbody2D>();

            rbHanler.position = spawnPoint + new Vector2(rowCounter * minimumSpace, columnCounter * minimumSpace) - offset;
            rbHanler.rotation = Random.Range(0f, 360f);

            Debug.Log("New object position: " + rbHanler.position);

            rowCounter++;
        }

        waveCount += increment;
        return true;
    }

    public bool EndGameCondition() {
        try {
            if (playerHandler) {
                if (playerHandler.gameObject.GetComponent<ShipScript>().shipLife < 0) return true;
            }
        } catch {
            return true;
        }
        return false;
    }

    public float GetCountdown() {
        return currentTime;
    }

    int GetNextWaveEnemyCount() {
        return waveCount;
    }

}
