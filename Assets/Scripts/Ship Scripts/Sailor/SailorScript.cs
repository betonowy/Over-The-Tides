using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailorScript : MonoBehaviour {

    public enum SailorType {
        SAILOR_DEFAULT,
        SAILOR_PLEASE_VIOLATE_ME,
        SAILOR_YES_PLEASE_HOLD_ME_IN_YOUR_ARMS,
        SAILOR_I_DONT_WANT_TO_LIVE_ANYMORE,
        SAILOR_O_O_O_O_OO_OONI_CHAAAN
    }

    Rigidbody2D rBody;
    ShipScript ship;
    private float alpha;
    Rigidbody2D shipBody;

    public Vector2 shipPosition;
    public Vector2 targetShipPosition;

    private SpriteRenderer spriteRenderer;
    public Sprite[] newSpriteFrontGreen;
    public Sprite[] newSpriteFrontWhite;
    public int whoIsIt;

    public SailorType sailorType { get; set; } = SailorType.SAILOR_DEFAULT;

    private bool changeSprite;
    public float walkSpeed;

    private AudioSource audioSteps;

    private NodeClass assignedNode;

    // Start is called before the first frame update
    void Start() {
        rBody = GetComponent<Rigidbody2D>();
        ship = transform.parent.gameObject.GetComponent<ShipScript>();
        shipBody = ship.gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = newSpriteFrontWhite[whoIsIt];
        changeSprite = false;
        audioSteps = GetComponents<AudioSource>()[0];
    }

    // Update is called once per frame
    void Update() {
        goToTarget();
        try {
            stayOnShip();
        } catch {
            Destroy(gameObject);
        }
        
        if(!changeSprite)
            spriteRenderer.sprite = newSpriteFrontWhite[whoIsIt];
        if (changeSprite)
            spriteRenderer.sprite = newSpriteFrontGreen[whoIsIt];
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
            if (!audioSteps.isPlaying) {
                audioSteps.pitch = Random.Range(0.95f, 1.05f);
                audioSteps.Play();
            }
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
        targetShipPosition = worldToShipCoordinates(coord);
    }

    public void Vomit() {
        Debug.Log("XD");
    }

    public void ChangeBoolSprite(bool newBool)
    {
        changeSprite = newBool;
    }

    public void AssignNode(NodeClass nc) {
        assignedNode = nc;
    }

    public void LeaveNode() {
        if (assignedNode != null) {
            assignedNode.SetSailor(null);
        }
        assignedNode = null;
    }
}
