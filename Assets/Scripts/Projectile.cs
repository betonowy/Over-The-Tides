using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;

    private Transform player;
    private Vector2 target;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        target = new Vector2(player.position.x, player.position.y);

    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        transform.Translate(speed * Time.deltaTime, 0,0);

        if (transform.position.x == target.x && transform.position.y == target.y) {
            DestroyProjectile();
        }
    }
    void DestroyProjectile() {
        Destroy(gameObject);
    }
}
