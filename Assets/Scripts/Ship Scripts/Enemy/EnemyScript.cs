using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class EnemyScript : MonoBehaviour {
    public float aiEncircleRadiusMax = 20;
    public float aiEncircleRadiusMin = 15;

    public float aiAttackDistance = 15;
    public float aiAttackAccuracyBeforeShoot = 0.9f;
    public float aiAttackFrontBlindSpot = 0.3f;

    public float aiCrewOrderPeriod = 3;
    public float aiMovementOrderPeriod = 2;
    public float aiAttackOrderPeriod = 2;
    public float aiOrderPeriodRandomize = 0.5f;

    private float aiCrewOrderTime = 0;
    private float aiMovementOrderTime = 0;
    private float aiAttackOrderTime = 0;

    private Vector2 aiTargetDirection;
    private bool aiPropell = false;
    private bool aiShootRight = false;
    private bool aiShootLeft = false;

    private GameObject[] sailors;
    private GameObject[] cannons;
    private GameObject[] masts;
    private GameObject[] steers;

    private ShipScript ship;

    // Start is called before the first frame update
    void Start() {
        ship = gameObject.GetComponent<ShipScript>();
        UpdateChildren();
        ResetAttackTimer();
        ResetCrewTimer();
        ResetMoveTimer();
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

    // Update is called once per frame
    void Update() {
        HandleOrders();
        ExecuteOrders();
    }

    void ResetCrewTimer() {
        aiCrewOrderTime = aiCrewOrderPeriod + Random.Range(0, aiOrderPeriodRandomize);
    }

    void ResetMoveTimer() {
        aiMovementOrderTime = aiMovementOrderPeriod + Random.Range(0, aiOrderPeriodRandomize);
    }

    void ResetAttackTimer() {
        aiAttackOrderTime = aiAttackOrderPeriod + Random.Range(0, aiOrderPeriodRandomize);
    }

    void ExecuteOrders() {
        ship.Turn(turnCorrection(aiTargetDirection) < 0);

        if (aiPropell)
            ship.Propeller(true);

        if (aiShootLeft)
            ship.ShootLeft();

        if (aiShootRight)
            ship.ShootRight();
    }

    void HandleOrders() {
        if (aiCrewOrderTime < 0) {
            ResetCrewTimer();
            CrewOrder();
        } else {
            aiCrewOrderTime -= Time.deltaTime;
        }

        if (aiMovementOrderTime < 0) {
            ResetMoveTimer();
            MoveOrder();
        } else {
            aiMovementOrderTime -= Time.deltaTime;
        }

        if (aiAttackOrderTime < 0) {
            ResetAttackTimer();
            AttackOrder();
        } else {
            aiAttackOrderTime -= Time.deltaTime;
        }
    }

    void CrewOrder() {
        float shootRightTargetPriority = 0;
        float shootLeftTargetPriority = 0;
        float mastTargetPriority;
        float steerTargetPriority;

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
                if (scalarRightHandTowardsTarget(targets[i]) > aiAttackFrontBlindSpot) {
                    shootRightTargetPriority += 2;
                } else if (scalarRightHandTowardsTarget(targets[i]) < -aiAttackFrontBlindSpot) {
                    shootLeftTargetPriority += 2;
                } else {
                    shootLeftTargetPriority += 1;
                    shootRightTargetPriority += 1;
                }
            } else if (getDistanceToTarget(targets[i]) < aiAttackDistance) {
                if (scalarRightHandTowardsTarget(targets[i]) > aiAttackFrontBlindSpot) {
                    shootRightTargetPriority += 1;
                } else if (scalarRightHandTowardsTarget(targets[i]) < -aiAttackFrontBlindSpot) {
                    shootLeftTargetPriority += 1;
                } else {
                    shootLeftTargetPriority += 0.5f;
                    shootRightTargetPriority += 0.5f;
                }
            }
        }

        if (getDistanceToTarget(closestTarget) > aiAttackDistance) {
            mastTargetPriority = 12;
            steerTargetPriority = 6;
        } else {
            mastTargetPriority = 1;
            steerTargetPriority = 1;
        }

        float prioritySummary = mastTargetPriority + steerTargetPriority + shootLeftTargetPriority + shootRightTargetPriority;
        float ratio = sailors.Length / prioritySummary;

        int[] cannonsL = new int[cannons.Length];

        for (int i = 0; i < cannonsL.Length; i++) {
            cannonsL[i] = -1;
            if (cannons[i].GetComponent<ParentConstraint>().GetRotationOffset(0).z > 0) {
                cannonsL[i] = 1;
            }
        }

        int ms = Mathf.FloorToInt(mastTargetPriority * ratio);
        int st = Mathf.FloorToInt(steerTargetPriority * ratio);
        int sl = Mathf.FloorToInt(shootLeftTargetPriority * ratio);
        int sr = Mathf.FloorToInt(shootRightTargetPriority * ratio);

        int sailorsLeft = sailors.Length - 1;

        int scriptIdiocyAndRetardSafeGuard = (cannons.Length + masts.Length + steers.Length) * 6;

        foreach (GameObject sailor in sailors) {
            sailor.GetComponent<SailorScript>().LeaveNode();
        }

        {
            int cannonIndex = 0;

            while (sl > 0 && sailorsLeft >= 0 && scriptIdiocyAndRetardSafeGuard-- > 0) {
                if (cannonsL[cannonIndex] > 0) {
                    if (cannons[cannonIndex].GetComponent<NodeScript>().AssignSailor(sailors[sailorsLeft])) {
                        sailorsLeft--;
                        sl--;
                    }
                }
                if (++cannonIndex >= cannons.Length) cannonIndex = 0;
            }

            while (sr > 0 && sailorsLeft >= 0 && scriptIdiocyAndRetardSafeGuard-- > 0) {
                if (cannonsL[cannonIndex] < 0) {
                    if (cannons[cannonIndex].GetComponent<NodeScript>().AssignSailor(sailors[sailorsLeft])) {
                        sailorsLeft--;
                        sr--;
                    }
                }
                if (++cannonIndex >= cannons.Length) cannonIndex = 0;
            }
        }

        {
            int mastIndex = 0;

            while (ms > 0 && sailorsLeft >= 0 && scriptIdiocyAndRetardSafeGuard-- > 0) {
                if (masts[mastIndex].GetComponent<NodeScript>().AssignSailor(sailors[sailorsLeft])) {
                    sailorsLeft--;
                    ms--;
                }
                if (++mastIndex >= masts.Length) mastIndex = 0;
            }
        }

        {
            int steerIndex = 0;

            while (st > 0 && sailorsLeft >= 0 && scriptIdiocyAndRetardSafeGuard-- > 0) {
                if (steers[steerIndex].GetComponent<NodeScript>().AssignSailor(sailors[sailorsLeft])) {
                    sailorsLeft--;
                    st--;
                }
                if (++steerIndex >= steers.Length) steerIndex = 0;
            }
        }

        {
            int cannonIndex = 0;
            int mastIndex = 0;
            int steerIndex = 0;

            while (sailorsLeft >= 0 && scriptIdiocyAndRetardSafeGuard-- > 0) {
                int cannonGuard = cannons.Length;
                while (cannonGuard-- > 0) {
                    bool assigned = false;
                    if (shootLeftTargetPriority > shootRightTargetPriority) {
                        if (cannonsL[cannonIndex] > 0) {
                            if (cannons[cannonIndex].GetComponent<NodeScript>().AssignSailor(sailors[sailorsLeft])) {
                                sailorsLeft--;
                                assigned = true;
                            }
                        }
                        if (++cannonIndex >= cannons.Length) cannonIndex = 0;
                    } else {
                        if (cannonsL[cannonIndex] < 0) {
                            if (cannons[cannonIndex].GetComponent<NodeScript>().AssignSailor(sailors[sailorsLeft])) {
                                sailorsLeft--;
                                assigned = true;
                            }
                        }
                        if (++cannonIndex >= cannons.Length) cannonIndex = 0;
                    }
                    if (assigned) break;
                }

                if (sailorsLeft < 0) break;

                int mastGuard = masts.Length;
                while (mastGuard-- > 0) {
                    bool assigned = false;
                    if (masts[mastIndex].GetComponent<NodeScript>().AssignSailor(sailors[sailorsLeft])) {
                        sailorsLeft--;
                        assigned = true;
                    }
                    if (++mastIndex >= masts.Length) mastIndex = 0;
                    if (assigned) break;
                }

                if (sailorsLeft < 0) break;

                int steerGuard = steers.Length;
                while (steerGuard-- > 0) {
                    bool assigned = false;
                    if (steers[steerIndex].GetComponent<NodeScript>().AssignSailor(sailors[sailorsLeft])) {
                        sailorsLeft--;
                        st--;
                    }
                    if (++steerIndex >= steers.Length) steerIndex = 0;
                    if (assigned) break;
                }
                // reverse cannon assignment if function continues
                shootLeftTargetPriority = -shootLeftTargetPriority;
            }
        }
    }

    void MoveOrder() {
        GameObject[] targets = getAllTargets();

        aiPropell = false;

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

        if (closestTarget != null) {
            aiTargetDirection = wishToGoDirection(closestTarget);
            aiPropell = true;
        }
    }

    void AttackOrder() {
        GameObject[] targets = getAllTargets();

        aiShootLeft = false;
        aiShootRight = false;

        if (targets.Length <= 0) {
            return;
        }

        foreach (GameObject target in targets) {
            if (getDistanceToTarget(target) < aiAttackDistance) {
                float scalar = scalarRightHandTowardsTarget(target);
                if (scalar > aiAttackAccuracyBeforeShoot)
                    aiShootRight = true;
                else if (scalar < -aiAttackAccuracyBeforeShoot)
                    aiShootLeft = true;
            }
        }
    }

    GameObject[] getAllTargets() {
        GameObject[] g = GameObject.FindGameObjectsWithTag("Ship");

        GameObject[] b = new GameObject[g.Length - 1];

        for (int i = 0, j = 0; i < g.Length; i++) {
            if (g[i] != gameObject) {
                b[j++] = g[i];
            }
        }

        return b;
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
