using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
    public float speed;

    public float stoppingDistance;

    public float retreaDistance;

    public float circleRadiusMin;
    public float circleRadiusMax;
    public float maxTurningForce;
    public float maxPropellerForce;


    private float timeBtwShots;

    public float startTimeBtwShots;


    public Transform player;

    public GameObject projectile;

    private Rigidbody2D myBody;



    // Start is called before the first frame update

    void Start() {

        player = GameObject.FindGameObjectWithTag("Player").transform;
        myBody = GetComponent<Rigidbody2D>();

        timeBtwShots = startTimeBtwShots;

    }



    // Update is called once per frame

    void Update() {

        /*
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance) {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        } else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreaDistance) {
            transform.position = this.transform.position;
        } else if (Vector2.Distance(transform.position, player.position) > retreaDistance) {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }
        */

        turn(turnCorrection(wishToGoDirection()) < 0);
        //turn(true);
        propeller(true);

        if (timeBtwShots <= 0) {
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        } else {
            timeBtwShots -= Time.deltaTime;
        }

    }

    Vector2 getVectorToPlayer() {
        return player.transform.position - gameObject.transform.position;
    }

    Vector2 getMyDirection() {
        float rotation = Mathf.Deg2Rad * myBody.rotation;
        return new Vector2(-Mathf.Sin(rotation), Mathf.Cos(rotation));
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

    float linearDecision(float input, float thresholdOne, float thresholdZero) {
        if (thresholdOne == thresholdZero) {
            if (input >= thresholdOne) return 1;
            else return 0;
        }

        float score = (input - thresholdZero) / (thresholdOne - thresholdZero);

        if (score >= 1) return 1;
        else if (score <= 0) return 0;

        return score;
    }

    Vector2 proportionalOfVectors(float proportion, Vector2 a, Vector2 b) {
        float antiProportion = 1 - proportion;
        return new Vector2(a.x * antiProportion + b.x * proportion, a.y * antiProportion + b.y * proportion);
    }

    Vector2 wishToGoDirection() {
        Vector2 distance = getVectorToPlayer();
        Vector2 directVector = distance.normalized;
        Vector2 encirclingVector;

        if (scalarRightHandTowardsTarget() > 0) encirclingVector = new Vector2(-directVector.y, directVector.x);
        else encirclingVector = new Vector2(directVector.y, -directVector.x);

        return proportionalOfVectors(linearDecision(distance.magnitude, circleRadiusMax, circleRadiusMin), encirclingVector, directVector);
    }

    float turnCorrection(Vector2 wishVector) {
        return Vector2.Dot(wishVector, getRightHandDirection());
    }

    void turn(bool direction) {
        if (direction) {
            myBody.AddTorque(maxTurningForce * Time.deltaTime);
        } else {
            myBody.AddTorque(-maxTurningForce * Time.deltaTime);
        }
    }
    void propeller(bool direction) {
        if (direction) {
            myBody.AddForce(getMyDirection() * maxPropellerForce * Time.deltaTime);
        } else {
            myBody.AddForce(-getMyDirection() * maxPropellerForce * Time.deltaTime);
        }
    }
}

