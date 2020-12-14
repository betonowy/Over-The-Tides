using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRippleSpawner : MonoBehaviour {
    public Vector2 spawnPoint;
    public float separation;

    public GameObject spawnObject;

    private Vector2 lastSpawnPoint;
    private Vector2 currentSpawnPoint;

    void Start() {
        currentSpawnPoint = GetSpawnPos();
        lastSpawnPoint = currentSpawnPoint;
    }

    // Update is called once per frame
    void Update() {
        currentSpawnPoint = GetSpawnPos();
        if ((lastSpawnPoint - currentSpawnPoint).magnitude > separation) {
            Spawn(currentSpawnPoint);
            lastSpawnPoint = currentSpawnPoint;
        }
    }

    void Spawn(Vector2 position) {
        GameObject newGO = Instantiate(spawnObject);
        newGO.transform.position = position;
    }

    Vector2 GetSpawnPos() {
        Vector2 relSpawn = spawnPoint;
        float angle = Mathf.Deg2Rad * (gameObject.transform.rotation.eulerAngles.z);
        relSpawn.x = spawnPoint.x * Mathf.Cos(angle) - spawnPoint.y * Mathf.Sin(angle);
        relSpawn.y = spawnPoint.y * Mathf.Cos(angle) + spawnPoint.x * Mathf.Sin(angle);
        return relSpawn + new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
    }
}
