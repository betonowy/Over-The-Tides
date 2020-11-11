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

    public Vector2[] cannonPositions;

    private float maxShipLife;

    // private GameObject healthBar;

    private SteerWheelScript steerWheelObject;
    private MastScript frontMastObject;
    private MastScript backMastObject;

    private GameObject[] cannons;

    public GameObject cannonPrefab;

    private bool[] cannonExistence;
    private bool cannonsTouched;
    public bool iTouchedCannons;

    private void Start() {
        cannonExistence = new bool[cannonPositions.Length];
        cannons = new GameObject[cannonPositions.Length];

        myBody = GetComponent<Rigidbody2D>();
        maxShipLife = shipLife;

        steerWheelObject = transform.Find("SteerWheel").GetComponent<SteerWheelScript>();
        frontMastObject = transform.Find("Mast").GetComponent<MastScript>();
        backMastObject = transform.Find("Mastback").GetComponent<MastScript>();

        initialCannons();
        UpdateCannons();
    }
    
    private void initialCannons() {
        cannonExistence[1] = true;
        cannonExistence[2] = true;
        cannonExistence[5] = true;
        cannonExistence[6] = true;
    }

    public void UpdateCannonsFromInventory(int index, bool val) {
        if (index >= 0 && index < cannonExistence.Length) {
            cannonExistence[index] = val;
            cannonsTouched = true;
            iTouchedCannons = true;
        }
    }

    public bool[] getCannonExistenceArray() {
        return cannonExistence;
    }
    private void UpdateCannons() {
        for (int i = 0; i < cannonPositions.Length; i++) {
            if (cannonExistence[i] && cannons[i] == null) {
                Debug.Log("Create cannon at index: " + i);
                CreateCannon(i);
            } else if (!cannonExistence[i] && cannons[i] != null) {
                Debug.Log("Destroy cannon at index: " + i);
                DestroyCannon(i);
            }
        }
    }

    private void CreateCannon(int index) {
        GameObject newCannon = Instantiate(cannonPrefab);
        newCannon.transform.SetParent(gameObject.transform);

        ParentConstraint cannonConstraint = newCannon.GetComponent<ParentConstraint>();
        {
            ConstraintSource source = new ConstraintSource();
            source.sourceTransform = transform;
            source.weight = 1.0f;
            cannonConstraint.SetSource(0, source);
        }

        cannonConstraint.SetTranslationOffset(0, cannonPositions[index]);

        if (cannonPositions[index].x > 0) {
            cannonConstraint.SetRotationOffset(0, new Vector3(0, 0, -90));
        } else {
            cannonConstraint.SetRotationOffset(0, new Vector3(0, 0, 90));
        }

        cannonConstraint.locked = true;

        cannons[index] = newCannon;
    }

    private void DestroyCannon(int index) {
        Destroy(cannons[index]);
        cannons[index] = null;
    }

    private void Update() {
        checkLife();
        if (cannonsTouched) {
            UpdateCannons();
            cannonsTouched = false;
        }
    }

    public void ShootLeft() {
        foreach (GameObject c in cannons) {
            if (c != null) {
                if (c.GetComponent<ParentConstraint>().GetRotationOffset(0).z > 0) {
                    c.SendMessage("shot");
                }
            }
        }
    }

    public void ShootRight() {
        foreach (GameObject c in cannons) {
            if (c != null) {
                if (c.GetComponent<ParentConstraint>().GetRotationOffset(0).z < 0) {
                    c.SendMessage("shot");
                }
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
        gameObject.GetComponents<AudioSource>()[1].Play();
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
