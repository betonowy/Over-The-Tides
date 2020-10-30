using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectPlayer : MonoBehaviour
{
    public float speed = 5f;

    private Transform enemy;
    private Vector2 target;

    void Start() {
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;

        target = new Vector2(enemy.position.x, enemy.position.y);

    }

    // Update is called once per frame
    void Update() {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (transform.position.x == target.x && transform.position.y == target.y) {
            DestroyProjectile();
        }
    }
    void DestroyProjectile() {
        Destroy(gameObject);
    }
}
