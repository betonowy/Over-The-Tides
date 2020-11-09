using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ShipScript : MonoBehaviour
{
    Rigidbody2D myBody;
    //CameraScript pCam;
    //private bool orderMode = true;

    // UI elements
    //public GameObject moveTargetMark; // prefab
    //private GameObject activeMoveMark;
    
    //public Vector3 targetPosition;
    //public bool reachedTarget = true;
    //public float distanceBeforeTargetReached = 0.5f;

    public enum teamEnum {
        teamRed,
        teamBlue,
        FFA
    }

    public teamEnum team;

    public float maxTurningForce;
    public float maxPropellerForce;

    public float shipLife = 100;
    private float maxShipLife;

    // private GameObject healthBar;

    private SteerWheelScript steerWheelObject;
    private MastScript frontMastObject;
    private MastScript backMastObject;

    private GameObject[] cannons;

    private void Start() {
        myBody = GetComponent<Rigidbody2D>();
        maxShipLife = shipLife;
        steerWheelObject = transform.Find("SteerWheel").GetComponent<SteerWheelScript>();
        frontMastObject = transform.Find("Mast").GetComponent<MastScript>();
        backMastObject = transform.Find("Mastback").GetComponent<MastScript>();
        UpdateCannons();
    }

    private void UpdateCannons() {
        int childCount = gameObject.transform.childCount;

        GameObject[] children = new GameObject[childCount];

        int cannonCount = 0;

        for (int i = 0; i < childCount; i++) {
            children[i] = gameObject.transform.GetChild(i).gameObject;
            if (children[i].name.StartsWith("cannon")) {
                cannonCount++;
            }
        }

        cannons = new GameObject[cannonCount];

        for (int i = 0; i < childCount; i++) {
            if (children[i].name.StartsWith("cannon")) {
                cannons[--cannonCount] = children[i];
            }
        }
    }

    private void Update() {
        checkLife();
    }

    public void ShootLeft() {
        foreach (GameObject c in cannons) {
            if (c.GetComponent<ParentConstraint>().GetRotationOffset(0).z > 0) {
                c.SendMessage("shot");
            }
        }
    }

    public void ShootRight() {
        foreach (GameObject c in cannons) {
            if (c.GetComponent<ParentConstraint>().GetRotationOffset(0).z < 0) {
                c.SendMessage("shot");
            }
        }
    }

    public Vector2 GetMyDirection() {
        float rotation = Mathf.Deg2Rad * myBody.rotation;
        return new Vector2(-Mathf.Sin(rotation), Mathf.Cos(rotation));
    }

    public Vector2 GetRightHandDirection() {
        Vector2 twist = GetMyDirection();
        return new Vector2(twist.y, -twist.x);
    }

    public float TurnCorrection(Vector2 wishVector) {
        return Vector2.Dot(wishVector, GetRightHandDirection());
    }

    public void Turn(bool direction) {
        if (direction) {
            myBody.AddTorque(maxTurningForce * Time.deltaTime * turnModifier());
        } else {
            myBody.AddTorque(-maxTurningForce * Time.deltaTime * turnModifier());
        }
    }

    public void Propeller(bool direction) {
        if (direction) {
            myBody.AddForce(GetMyDirection() * maxPropellerForce * Time.deltaTime * speedModifier());
        } else {
            myBody.AddForce(-GetMyDirection() * maxPropellerForce * Time.deltaTime * speedModifier());
        }
    }

    public void Damage(float value) {
        shipLife -= value;
    }

    private void checkLife() {
        OnCheckLife();
        if (shipLife < 0) {
            //FindObjectOfType<LevelManager>().SendMessage("OnPlayerDeath");
            OnPlayerDeath();
            Destroy(gameObject);
        }
    }

    public void OnCheckLife() {

    }

    public void OnPlayerDeath() {

    }

    private bool steerWheelManned() {
        if (steerWheelObject.getNodeScript().ReadyCrewCount() > 0)
            return true;
        return false;
    }

    private float speedModifier() {
        float maxMastManned = frontMastObject.getNodes().countNodes() + backMastObject.getNodes().countNodes();
        float currentlyManned = frontMastObject.getNodes().ReadyCrewCount() + backMastObject.getNodes().ReadyCrewCount();
        return currentlyManned / maxMastManned;
    }

    private float turnModifier() {
        NodeScript script = steerWheelObject.getNodeScript();
        return (float)script.ReadyCrewCount() / script.countNodes() * myBody.velocity.magnitude;
    }
}
