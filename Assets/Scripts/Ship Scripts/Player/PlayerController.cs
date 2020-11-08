using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;
//using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    Rigidbody2D myBody;
    CameraScript pCam;
    private bool orderMode = true;

    // UI elements
    public GameObject moveTargetMark; // prefab
    private GameObject activeMoveMark;

    public Vector3 targetPosition;
    public bool reachedTarget = true;
    public float distanceBeforeTargetReached = 0.5f;

    public float maxTurningForce;
    public float maxPropellerForce;

    public float shipLife = 100;
    private float maxShipLife;

    private GameObject healthBar;

    private SteerWheelScript steerWheelObject;
    private MastScript frontMastObject;
    private MastScript backMastObject;

    private void Start() {
        myBody = GetComponent<Rigidbody2D>();
        pCam = FindObjectOfType<CameraScript>();
        healthBar = GameObject.Find("Health bar");
        healthBar.SendMessage("setMaxHealth", shipLife);
        maxShipLife = shipLife;
        steerWheelObject = transform.Find("SteerWheel").GetComponent<SteerWheelScript>();
        frontMastObject = transform.Find("Mast").GetComponent<MastScript>();
        backMastObject = transform.Find("Mastback").GetComponent<MastScript>();
    }

    private void Update() {
        checkLife();

        if (!reachedTarget) {
            Propeller(true);
            Turn(TurnCorrection(WishToGoDirection()) < 0);
        }

        if ((targetPosition - gameObject.transform.position).magnitude < distanceBeforeTargetReached) {
            reachedTarget = true;
            if (activeMoveMark != null) {
                Destroy(activeMoveMark);
                activeMoveMark = null;
            }
        }

        MouseMovement();
        Shooting();
    }

    private void Shooting() {
        if (Input.GetKey(KeyCode.Q)) {
            GameObject cannon = GameObject.Find("cannonNoFire (1)");
            GameObject cannon2 = GameObject.Find("cannonNoFire (0)");
            cannon.SendMessage("shot");
            cannon2.SendMessage("shot");
        }
        if (Input.GetKey(KeyCode.E)) {
            GameObject cannon = GameObject.Find("cannonNoFire (2)");
            GameObject cannon2 = GameObject.Find("cannonNoFire (3)");
            cannon.SendMessage("shot");
            cannon2.SendMessage("shot");
        }
    }

    private void MouseMovement() {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePos = Input.mousePosition;
            // check if mouse inside viewport
            Vector2 viewport = Camera.allCameras[0].ScreenToViewportPoint(mousePos);
            bool insideViewport = true;
            if (viewport.x > 1f || viewport.x < 0f || viewport.y > 1f || viewport.y < 0f)
                insideViewport = false;
            // point ship where you want to go
            if (insideViewport) {
                targetPosition = Camera.allCameras[0].ScreenToWorldPoint(Input.mousePosition);
                targetPosition.z = gameObject.transform.position.z;
                reachedTarget = false;
                if (activeMoveMark != null) {
                    Destroy(activeMoveMark);
                }
                activeMoveMark = Instantiate(moveTargetMark);
                activeMoveMark.transform.position = targetPosition;
            }
        }
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
            myBody.AddTorque(maxTurningForce * Time.deltaTime * turnModifier());
        } else {
            myBody.AddTorque(-maxTurningForce * Time.deltaTime * turnModifier());
        }
    }

    void Propeller(bool direction) {
        if (direction) {
            myBody.AddForce(GetMyDirection() * maxPropellerForce * Time.deltaTime * speedModifier());
        } else {
            myBody.AddForce(-GetMyDirection() * maxPropellerForce * Time.deltaTime * speedModifier());
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

    bool steerWheelManned() {
        if (steerWheelObject.getNodeScript().ReadyCrewCount() > 0)
            return true;
        return false;
    }

    float speedModifier() {
        float maxMastManned = frontMastObject.getNodes().countNodes() + backMastObject.getNodes().countNodes();
        float currentlyManned = frontMastObject.getNodes().ReadyCrewCount() + backMastObject.getNodes().ReadyCrewCount();
        return currentlyManned / maxMastManned;
    }

    float turnModifier() {
        NodeScript script = steerWheelObject.getNodeScript();
        return (float)script.ReadyCrewCount() / script.countNodes();
    }
}