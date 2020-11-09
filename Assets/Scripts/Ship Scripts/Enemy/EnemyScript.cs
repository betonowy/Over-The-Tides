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

    // Start is called before the first frame update
    void Start() {
        UpdateChildren();
        Debug.Log("Sailors: " + sailors.Length);
        Debug.Log("Cannons: " + cannons.Length);
        Debug.Log("Masts: " + masts.Length);
        Debug.Log("Steers: " + steers.Length);
    }

    private void UpdateChildren() {
        int childCount = gameObject.transform.childCount;

        Debug.Log("childCount: " + childCount);

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
}
