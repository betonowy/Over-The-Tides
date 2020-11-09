using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
    public float aiEncircleRadiusMax = 20;
    public float aiEncircleRadiusMin = 15;

    public float aiAttackDistance = 15;
    public float aiAttackAngle = 15;

    public float aiCrewOrderPeriod = 3;
    public float aiMovementOrderPeriod = 2;
    public float aiAttackOrderPeriod = 2;
    public float aiOrderPeriodRandomize = 0.5f;

    private float aiCrewOrderTime = 0;
    private float aiMovementOrderTime = 0;
    private float aiAttackOrderTime = 0;

    private float aiPriorityShootRight = 0;
    private float aiPriorityShootLeft = 0;
    private float aiPriorityMast = 0;
    private float aiPrioritySteer = 0;

    public float aiPriorityRaiseSpeed = 0.1f;
    public float aiPrioritySettleSpeed = 0.1f;

    private Vector2 aiTargetDirection;
    private bool aiShootRight = false;
    private bool aiShootLeft = false;

    private bool initialAssignment = true;

    private GameObject[] sailors;
    private GameObject[] cannons;
    private GameObject[] masts;
    private GameObject[] steers;

    private ShipScript ship;

    // Start is called before the first frame update
    void Start() {
        UpdateChildren();
    }

    private void UpdateChildren() {
        int childCount = gameObject.transform.childCount;

        GameObject[] children = new GameObject[childCount];

        int cannonCount = 0;
        int mastCount = 0;
        int steerCount = 0;
        int sailorCount = 0;

        for (int i = 0; i < childCount; i++) {
            children[i] = gameObject.transform.GetChild(i).gameObject;
            if (children[i].name.StartsWith("cannon")) {
                cannonCount++;
            } else if (children[i].name.StartsWith("Mast")) {
                mastCount++;
            } else if (children[i].name.StartsWith("Steer")) {
                steerCount++;
            } else if (children[i].name.StartsWith("Sailor")) {
                sailorCount++;
            }
        }

        cannons = new GameObject[cannonCount];
        masts = new GameObject[mastCount];
        steers = new GameObject[steerCount];
        sailors = new GameObject[sailorCount];

        for (int i = 0; i < childCount; i++) {
            if (children[i].name.StartsWith("cannon")) {
                cannons[--cannonCount] = children[i];
            } else if (children[i].name.StartsWith("Mast")) {
                masts[--mastCount] = children[i];
            } else if (children[i].name.StartsWith("Steer")) {
                steers[--steerCount] = children[i];
            } else if (children[i].name.StartsWith("Sailor")) {
                sailors[--sailorCount] = children[i];
            }
        }
    }

    private void TestAssignment() {
        int sailorsLeft = sailors.Length;
        for (int i = 0; i < cannons.Length && sailorsLeft > 0; i++) {
            cannons[i].GetComponent<NodeScript>().AssignSailor(sailors[--sailorsLeft]);
        }
        int divide = sailorsLeft / 2;
        for (int i = 0; i < divide && sailorsLeft > 0 && i < steers.Length; i++) {
            steers[i].GetComponent<NodeScript>().AssignSailor(sailors[--sailorsLeft]);
        }
        {
            int mastsAssigned = 0;
            while (sailorsLeft > 0 && mastsAssigned < masts.Length) {
                masts[mastsAssigned++].GetComponent<NodeScript>().AssignSailor(sailors[--sailorsLeft]);
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (initialAssignment) {
            initialAssignment = false;
            TestAssignment();
        }
    }

    void calculateCrewPriority() {
        float shootRightTargetPriority = 0;
        float shootLeftTargetPriority = 0;
        float mastTargetPriority = 0;
        float steerTargetPriority = 0;

        GameObject[] targets = getAllTargets();

        if (targets.Length <= 0) {
            return;
        }

        GameObject closestTarget = targets[0];
        {
            float minDist = getDistanceToTarget(targets[0]);
            for (int i = 1; i < targets.Length; i++) {
                float dist = getDistanceToTarget(targets[i]);
                if (dist < minDist) {
                    minDist = dist;
                    closestTarget = targets[i];
                }
            }
        }

        for (int i = 0; i < targets.Length; i++) {
            if (getDistanceToTarget(targets[i]) < aiAttackDistance / 2) {
                if (scalarRightHandTowardsTarget(targets[i]) > 0) {
                    shootRightTargetPriority += 2;
                } else {
                    shootLeftTargetPriority += 2;
                }
            } else if (getDistanceToTarget(targets[i]) < aiAttackDistance) {
                if (scalarRightHandTowardsTarget(targets[i]) > 0) {
                    shootRightTargetPriority += 1;
                } else {
                    shootLeftTargetPriority += 1;
                }
            }
        }

        if (getDistanceToTarget(closestTarget) > aiAttackDistance) {
            mastTargetPriority = 10;
            steerTargetPriority = 5;
        } else {
            mastTargetPriority = 1;
            steerTargetPriority = 1;
        }

        int sailorCount = sailors.Length;
        float prioritySummary = mastTargetPriority + steerTargetPriority + shootLeftTargetPriority + shootRightTargetPriority;
        float ratio = sailors.Length / prioritySummary;
    }

    GameObject[] getAllTargets() {
        return GameObject.FindGameObjectsWithTag("Ship");
    }

    float getDistanceToTarget(GameObject g) {
        Vector2 targetPos = g.transform.position - gameObject.transform.position;
        return targetPos.magnitude;
    }







    Vector2 getVectorToTarget(GameObject g) {
        return g.transform.position - gameObject.transform.position;
    }

    Vector2 getMyDirection() {
        float rotation = Mathf.Deg2Rad * gameObject.GetComponent<Rigidbody2D>().rotation;
        return new Vector2(-Mathf.Sin(rotation), Mathf.Cos(rotation));
    }

    Vector2 getRightHandDirection() {
        Vector2 twist = getMyDirection();
        return new Vector2(twist.y, -twist.x);
    }

    float scalarTowardsTarget(GameObject g) {
        return Vector2.Dot(getMyDirection(), getVectorToTarget(g).normalized);
    }

    float scalarRightHandTowardsTarget(GameObject g) {
        return Vector2.Dot(getRightHandDirection(), getVectorToTarget(g).normalized);
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

    Vector2 wishToGoDirection(GameObject g) {
        Vector2 distance = getVectorToTarget(g);
        Vector2 directVector = distance.normalized;
        Vector2 encirclingVector;

        if (scalarRightHandTowardsTarget(g) > 0) encirclingVector = new Vector2(-directVector.y, directVector.x);
        else encirclingVector = new Vector2(directVector.y, -directVector.x);

        return proportionalOfVectors(linearDecision(distance.magnitude, aiEncircleRadiusMax, aiEncircleRadiusMin), encirclingVector, directVector);
    }

    float turnCorrection(Vector2 wishVector) {
        return Vector2.Dot(wishVector, getRightHandDirection());
    }
}
