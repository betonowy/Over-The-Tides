using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;
//using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    Rigidbody2D myBody;
    PlayerCamera pCam;
    private bool orderMode = true;

    public Vector3 targetPosition;
    public bool reachedTarget = true;
    public float distanceBeforeTargetReached = 0.5f;

    public GameObject projectile;
    private float timeBtwShoots;
    public float startTimeBtwShoots;

    public Transform shotPointLeft;
    public Transform shotPointRight;

    public float maxTurningForce;
    public float maxPropellerForce;

    private void Start() {
        myBody = GetComponent<Rigidbody2D>();
        pCam = FindObjectOfType<PlayerCamera>();
        timeBtwShoots = startTimeBtwShoots;
        shotPointLeft.transform.Rotate(0f, 0f, 90f, Space.World);
        shotPointRight.transform.Rotate(0f, 0f, -90f, Space.World);
    }

    private void Update() {
        if (!reachedTarget) {
            propeller(true);
            turn(turnCorrection(wishToGoDirection()) < 0);
        }

        if ((targetPosition - gameObject.transform.position).magnitude < distanceBeforeTargetReached)
            reachedTarget = true;

        mouseMovement();
        shooting();
    }

    private void shooting() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            if (timeBtwShoots <= 0) {
                //Instantiate(projectile, transform.position, Quaternion.identity);
                Instantiate(projectile, shotPointLeft.position, shotPointLeft.rotation);
                timeBtwShoots = startTimeBtwShoots;
            }

        } else if (Input.GetKeyDown(KeyCode.E)) {
            if (timeBtwShoots <= 0) {
                //Instantiate(projectile, transform.position, Quaternion.identity);
                Instantiate(projectile, shotPointRight.position, shotPointRight.rotation);
                timeBtwShoots = startTimeBtwShoots;
            }

        } else timeBtwShoots -= Time.deltaTime;
    }

    private void mouseMovement() {
        if (!orderMode) {
            if (Input.GetMouseButtonDown(0)) {
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition.z = gameObject.transform.position.z;
                reachedTarget = false;
            }
        } else {
            if (Input.GetMouseButtonDown(0)) {
                GameObject[] sailors = GameObject.FindGameObjectsWithTag("Sailor");
                var mouse = Input.mousePosition;
                mouse.z = 10;
                Vector2 sailorTarget = Camera.allCameras[0].ScreenToWorldPoint(mouse);
                for (int i = 0; i < sailors.Length; i++) {
                    sailors[i].SendMessage("setTargetWorldSpace", sailorTarget);
                }
            }
        }
    }

    Vector2 getVectorToPlayer() {
        return targetPosition - gameObject.transform.position;
    }

    Vector2 getMyDirection() {
        float rotation = Mathf.Deg2Rad * myBody.rotation;
        return new Vector2(-Mathf.Sin(rotation), Mathf.Cos(rotation));
    }

    Vector2 getRightHandDirection() {
        Vector2 twist = getMyDirection();
        return new Vector2(twist.y, -twist.x);
    }

    Vector2 wishToGoDirection() {
        Vector2 distance = getVectorToPlayer();
        Vector2 directVector = distance.normalized;

        return directVector;
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

    void setOrderMode(bool val) {
        orderMode = val;
    }


}