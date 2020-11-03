﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : MonoBehaviour
{
    public GameObject ballTemplate;
    public float ballSpawnDistance = 0.1f;
    public float ballSpeed = 3;
    public float cooldownTime = 3;


    private Vector2 transformedSpawnPoint = new Vector2();
    private Vector2 spawnVelocityVector = new Vector2();
    private float cooldown = 0;

    private Rigidbody2D cannonRB;
    // Start is called before the first frame update
    void Start()
    {
        cannonRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cooldown > 0) {
            cooldown -= Time.deltaTime;
        }
    }

    void updateSpawnPointAndVelocity() {
        float rotation = cannonRB.rotation * Mathf.Deg2Rad;

        transformedSpawnPoint.x = -Mathf.Sin(rotation);
        transformedSpawnPoint.y = Mathf.Cos(rotation);

        spawnVelocityVector = transformedSpawnPoint * ballSpeed + cannonRB.velocity;

        transformedSpawnPoint *= ballSpawnDistance;
    }

    void shot() {
        updateSpawnPointAndVelocity();

        GameObject spawnedBall = Instantiate(ballTemplate, gameObject.transform.position, gameObject.transform.rotation);
        Rigidbody2D ballRigidbody = spawnedBall.GetComponent<Rigidbody2D>();

        ballRigidbody.position += transformedSpawnPoint;
        ballRigidbody.velocity = spawnVelocityVector;

        cooldown += cooldownTime;
    }
}
