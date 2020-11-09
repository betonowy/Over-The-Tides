using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CannonScript : MonoBehaviour {

    public GameObject ballTemplate;

    public float ballSpawnDistance = 0.2f;
    public float ballSpeed = 2;
    public float cooldownTime = 3;
    public float power = 20;

    public Vector2[] nodes;

    private Vector2 transformedSpawnPoint = new Vector2();
    private Vector2 spawnVelocityVector = new Vector2();
    private float cooldown = 0;

    private NodeScript ns;
    private Rigidbody2D cannonRB;

    // Start is called before the first frame update
    void Start() {
        cannonRB = GetComponent<Rigidbody2D>();
        ns = gameObject.AddComponent<NodeScript>();
        ns.SetParent(gameObject);
        for (int i = 0; i < nodes.Length; i++) {
            ns.CreateRelativeNode(nodes[i]);
        }
        cooldown = cooldownTime;
    }

    // Update is called once per frame
    void Update() {
        cooldown -= Time.deltaTime * (float)ns.ReadyCrewCount() / (float)nodes.Length;
        Debug.Log(cooldown);
    }

    void updateSpawnPointAndVelocity() {
        float rotation = cannonRB.rotation * Mathf.Deg2Rad;

        transformedSpawnPoint.x = -Mathf.Sin(rotation);
        transformedSpawnPoint.y = Mathf.Cos(rotation);

        spawnVelocityVector = transformedSpawnPoint * ballSpeed + cannonRB.velocity;
        transformedSpawnPoint *= ballSpawnDistance;
    }

    void shot() {
        if (cooldown <= 0) {
            updateSpawnPointAndVelocity();

            GameObject spawnedBall = Instantiate(ballTemplate, gameObject.transform.position, gameObject.transform.rotation);
            Rigidbody2D ballRigidbody = spawnedBall.GetComponent<Rigidbody2D>();

            ballRigidbody.position += transformedSpawnPoint;
            ballRigidbody.velocity = spawnVelocityVector;

            spawnedBall.SendMessage("SetInitialSpeed", spawnVelocityVector);
            cooldown = cooldownTime;
        }
    }
}
