using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
    public float speed;

    public float stoppingDistance;

    public float retreaDistance;



    private float timeBtwShots;

    public float startTimeBtwShots;


    public Transform player;

    public GameObject projectile;

    private GameObject meTheEnemy;
    private Rigidbody2D myBody;



    // Start is called before the first frame update

    void Start() {

        player = GameObject.FindGameObjectWithTag("Player").transform;
        meTheEnemy = GetComponent<GameObject>();
        myBody = GetComponent<Rigidbody2D>();

        timeBtwShots = startTimeBtwShots;

    }



    // Update is called once per frame

    void Update() {

        if (Vector2.Distance(transform.position, player.position) > stoppingDistance) {

            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        } else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreaDistance) {

            transform.position = this.transform.position;

        } else if (Vector2.Distance(transform.position, player.position) > retreaDistance) {

            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);

        }



        if (timeBtwShots <= 0) {

            Instantiate(projectile, transform.position, Quaternion.identity);

            timeBtwShots = startTimeBtwShots;

        } else {

            timeBtwShots -= Time.deltaTime;

        }

    }

    Vector2 getVectorToPlayer() {
        return player.transform.position - meTheEnemy.transform.position;
    }

    Vector2 getMyDirection() {
        return new Vector2(Mathf.Sin(myBody.rotation), Mathf.Cos(myBody.rotation)); ;
    }

    Vector2 getRightHandDirection() {
        Vector2 twist = getMyDirection();
        return new Vector2(twist.y, -twist.x);
    }

    float scalarTowardsTarget() {
        return Vector2.Dot(getMyDirection(), getVectorToPlayer().normalized);
    }

    float scalarRightHandTowardsTarget() {
        return Vector2.Dot(getRightHandDirection(), getVectorToPlayer().normalized);
    }
}

