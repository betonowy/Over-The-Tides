using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectPlayer : MonoBehaviour
{
    public float speed = 5f;

    private Transform enemy;
    private Vector2 target;

    bool shotDirection;
    
    void Start() {
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;

        target = new Vector2(enemy.position.x, enemy.position.y);
        //Invoke("DestroyProjectile", lifetime);
    }

    // Update is called once per frame
    void Update() {
        //if(shotDirection)
        //    transform.Translate(transform.right * -speed * Time.deltaTime);
        //else if(!shotDirection)
            transform.Translate(0, speed * Time.deltaTime, 0);


        //if (transform.position.x == target.x && transform.position.y == target.y) {
        //    DestroyProjectile();
        //}
    }

    public void shotLeft() {
        shotDirection = true;
    }

    public void shotRight() {
        shotDirection = false;
    }

    void DestroyProjectile() {
        Destroy(gameObject);
    }
}
