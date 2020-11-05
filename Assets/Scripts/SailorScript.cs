using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailorScript : MonoBehaviour {
    Rigidbody2D rBody;
    PlayerController ship;
    private float alpha;
    Rigidbody2D shipBody;

    public Vector2 shipPosition;
    public Vector2 targetShipPosition;

    public float walkSpeed;

    // Start is called before the first frame update
    void Start() {
        rBody = GetComponent<Rigidbody2D>();
        ship = FindObjectOfType<PlayerController>();
        shipBody = ship.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        goToTarget();
        stayOnShip();
    }

    private void stayOnShip() {
        alpha = shipBody.rotation * Mathf.Deg2Rad;
        gameObject.transform.position =
            ship.gameObject.transform.position + new Vector3(
            Mathf.Cos(alpha) * shipPosition.x - Mathf.Sin(alpha) * shipPosition.y,
            Mathf.Cos(alpha) * shipPosition.y + Mathf.Sin(alpha) * shipPosition.x
            );
        rBody.rotation = shipBody.rotation;
    }

    private void goToTarget() {
        Vector2 walkDir = targetShipPosition - shipPosition;
        Vector2 stepVector = walkDir.normalized * walkSpeed * Time.deltaTime;
        if (walkDir.magnitude > stepVector.magnitude) {
            shipPosition += stepVector;
        } else {
            shipPosition = targetShipPosition;
        }
    }

    Vector2 worldToShipCoordinates(Vector2 coord) {
        alpha = -shipBody.rotation * Mathf.Deg2Rad;
        Vector3 newCoord = coord;
        newCoord -= ship.gameObject.transform.position;
        return new Vector3(
            Mathf.Cos(alpha) * newCoord.x - Mathf.Sin(alpha) * newCoord.y,
            Mathf.Cos(alpha) * newCoord.y + Mathf.Sin(alpha) * newCoord.x
            );
    }

    private void setTargetWorldSpace(Vector2 coord) {
        Debug.Log("coords before trans: " + coord);
        targetShipPosition = worldToShipCoordinates(coord);
        Debug.Log("coords after trans: " + targetShipPosition);
    }

    public void Vomit() {
        Debug.Log("XD");
    }
}
