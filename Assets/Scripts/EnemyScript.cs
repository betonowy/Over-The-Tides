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

    public float shootingRange = 5f;

    public Transform castPoint0;
    public Transform castPoint1;
    public Transform castPoint2;
    public Transform castPoint3;

    


    // Start is called before the first frame update

    void Start() {

        player = GameObject.FindGameObjectWithTag("Player").transform;
        myBody = GetComponent<Rigidbody2D>();

        timeBtwShots = startTimeBtwShots;

    }

    // Update is called once per frame
    void Update() {
        turn(turnCorrection(wishToGoDirection()) < 0);
        propeller(true);

        sniping();
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
    private void sniping() {
        bool flag;
        if (isInRange(castPoint0)) {
            shooting(castPoint0);
        } else if (isInRange(castPoint0)) {
            shooting(castPoint0);
        } else if (isInRange(castPoint1)) {
            shooting(castPoint1);
        } else if (isInRange(castPoint2)) {
            shooting(castPoint2);
        } else if (isInRange(castPoint3)) {
            shooting(castPoint3);
        }
        timeBtwShots -= Time.deltaTime;

    }
    private bool isInRange(Transform castPoint) {
        bool flag = false;
        float distance = shootingRange;
        Vector2 endPos = castPoint.position + Vector3.right * distance;

        RaycastHit2D ray = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Combat"));

        if (ray.collider != null) {
            if(ray.collider.gameObject.CompareTag("Player")) {
                flag = true;
            } else {
                flag = false;
            }
            Debug.DrawLine(castPoint.position, ray.point, Color.yellow);
        } else {
            Debug.DrawLine(castPoint.position, ray.point, Color.blue);
        }
        return flag;
    }

    private void shooting(Transform castPoint) {
        if (timeBtwShots <= 0) {
            Instantiate(projectile, castPoint.position, castPoint.rotation);
            timeBtwShots = startTimeBtwShots;
        }
    }
}

