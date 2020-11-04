using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
    public float circleRadiusMin;
    public float circleRadiusMax;
    public float maxTurningForce;
    public float maxPropellerForce;

    private float timeBtwShots;
    public float startTimeBtwShots;

    public Transform player;
    public GameObject projectile;
    private Rigidbody2D myBody;

    public float shootingRange;

    public Transform castPoint0;
    public Transform castPoint1;
    public Transform castPoint2;
    public Transform castPoint3;
    public LayerMask mask;

    public float shipLife = 100;

    // Start is called before the first frame update

    void Start() {

        player = GameObject.FindGameObjectWithTag("Player").transform;
        myBody = GetComponent<Rigidbody2D>();

        timeBtwShots = startTimeBtwShots;

    }

    // Update is called once per frame
    void Update() {
        checkLife();

        turn(turnCorrection(wishToGoDirection()) < 0);
        propeller(true);

        shooting();
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
    private void shooting() {

        isInRange(castPoint0);
        isInRange(castPoint1);
        isInRange(castPoint2);
        isInRange(castPoint3);


        timeBtwShots -= Time.deltaTime;

    }
    private void isInRange(Transform castPoint) {
        RaycastHit2D hit = Physics2D.Raycast(castPoint.position, castPoint.transform.TransformDirection(Vector3.right), shootingRange, mask);

        if (hit.collider != null) {
            shot(castPoint);
        }
        Debug.DrawRay(castPoint.position, castPoint.transform.TransformDirection(Vector3.right) * shootingRange, Color.blue);
    }

    private void shot(Transform castPoint) {
        if (timeBtwShots <= 0) {
            Instantiate(projectile, castPoint.position, castPoint.rotation);
            timeBtwShots = startTimeBtwShots;
        }
    }

    void Damage(float value) {
        shipLife -= value;
    }

    void checkLife() {
        if (shipLife < 0) {
            Destroy(gameObject);
        }
    }
}

