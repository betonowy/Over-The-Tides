using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;
//using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    Rigidbody2D myBody;
    CameraScript pCam;
    private bool orderMode = true;

    public Vector3 targetPosition;
    public bool reachedTarget = true;
    public float distanceBeforeTargetReached = 0.5f;

    public GameObject projectile;
    private float timeBtwShoots;
    public float startTimeBtwShoots;

    public float maxTurningForce;
    public float maxPropellerForce;

    public float shipLife = 100;
    private float maxShipLife;

    private GameObject healthBar;

    private void Start() {
        myBody = GetComponent<Rigidbody2D>();
        pCam = FindObjectOfType<CameraScript>();
        timeBtwShoots = startTimeBtwShoots;
        healthBar = GameObject.Find("Health bar");
        healthBar.SendMessage("setMaxHealth", shipLife);
        maxShipLife = shipLife;
    }

    private void Update() {
        checkLife();

        if (!reachedTarget) {
            Propeller(true);
            Turn(TurnCorrection(WishToGoDirection()) < 0);
        }

        if ((targetPosition - gameObject.transform.position).magnitude < distanceBeforeTargetReached)
            reachedTarget = true;

        MouseMovement();
        Shooting();
    }

    private void Shooting() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            if (timeBtwShoots <= 0) {
                GameObject cannon = GameObject.Find("cannonNoFire (1)");
                GameObject cannon2 = GameObject.Find("cannonNoFire (0)");
                cannon.SendMessage("shot");
                cannon2.SendMessage("shot");
                timeBtwShoots = startTimeBtwShoots;
            }

        } else if (Input.GetKeyDown(KeyCode.E)) {
            if (timeBtwShoots <= 0) {
                GameObject cannon = GameObject.Find("cannonNoFire (2)");
                GameObject cannon2 = GameObject.Find("cannonNoFire (3)");
                cannon.SendMessage("shot");
                cannon2.SendMessage("shot");
                timeBtwShoots = startTimeBtwShoots;
            }

        } else timeBtwShoots -= Time.deltaTime;
    }

    private void MouseMovement() {
        if (!orderMode) {
            if (Input.GetMouseButtonDown(0)) {
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition.z = gameObject.transform.position.z;
                reachedTarget = false;
            }
        } /*else {
            if (Input.GetMouseButtonDown(1)) {
                GameObject[] sailors = GameObject.FindGameObjectsWithTag("Sailor");
                var mouse = Input.mousePosition;
                mouse.z = 10;
                Vector2 sailorTarget = Camera.allCameras[0].ScreenToWorldPoint(mouse);
                for (int i = 0; i < sailors.Length; i++) {
                    sailors[i].SendMessage("setTargetWorldSpace", sailorTarget);
                }
            }
        }*/
    }

    Vector2 GetVectorToPlayer() {
        return targetPosition - gameObject.transform.position;
    }

    Vector2 GetMyDirection() {
        float rotation = Mathf.Deg2Rad * myBody.rotation;
        return new Vector2(-Mathf.Sin(rotation), Mathf.Cos(rotation));
    }

    Vector2 GetRightHandDirection() {
        Vector2 twist = GetMyDirection();
        return new Vector2(twist.y, -twist.x);
    }

    Vector2 WishToGoDirection() {
        Vector2 distance = GetVectorToPlayer();
        Vector2 directVector = distance.normalized;

        return directVector;
    }

    float TurnCorrection(Vector2 wishVector) {
        return Vector2.Dot(wishVector, GetRightHandDirection());
    }

    void Turn(bool direction) {
        if (direction) {
            myBody.AddTorque(maxTurningForce * Time.deltaTime);
        } else {
            myBody.AddTorque(-maxTurningForce * Time.deltaTime);
        }
    }

    void Propeller(bool direction) {
        if (direction) {
            myBody.AddForce(GetMyDirection() * maxPropellerForce * Time.deltaTime);
        } else {
            myBody.AddForce(-GetMyDirection() * maxPropellerForce * Time.deltaTime);
        }
    }

    void SetOrderMode(bool val) {
        orderMode = val;
    }

    void Damage(float value) {
        shipLife -= value;
        healthBar.SendMessage("setHealth", (shipLife < 0) ? 0 : shipLife);
    }

    void checkLife() {
        if (shipLife < 0) {
            FindObjectOfType<LevelManager>().SendMessage("OnPlayerDeath");
            Destroy(gameObject);
        }
    }
}