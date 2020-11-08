using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    CameraScript pCam;

    // UI elements
    public GameObject moveTargetMark; // prefab
    private GameObject activeMoveMark;

    public Vector3 targetPosition;
    public bool reachedTarget = true;
    public float distanceBeforeTargetReached = 0.5f;

    private GameObject healthBar;

    private ShipScript shipScript;

    // Start is called before the first frame update
    void Start()
    {
        shipScript = gameObject.GetComponent<ShipScript>();
        pCam = FindObjectOfType<CameraScript>();
        healthBar = GameObject.Find("Health bar");
        healthBar.SendMessage("setMaxHealth", shipScript.shipLife);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Shooting();
    }

    void Shooting() {
        if (Input.GetKey(KeyCode.Q))
            shipScript.ShootLeft();
        if (Input.GetKey(KeyCode.E))
            shipScript.ShootRight();
    }

    private void Movement() {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePos = Input.mousePosition;

            Vector2 viewport = Camera.allCameras[0].ScreenToViewportPoint(mousePos);
            bool insideViewport = true;
            if (viewport.x > 1f || viewport.x < 0f || viewport.y > 1f || viewport.y < 0f)
                insideViewport = false;

            if (insideViewport) {
                targetPosition = Camera.allCameras[0].ScreenToWorldPoint(mousePos);
                targetPosition.z = gameObject.transform.position.z;
                reachedTarget = false;
                if (activeMoveMark != null) {
                    Destroy(activeMoveMark);
                }
                activeMoveMark = Instantiate(moveTargetMark);
                activeMoveMark.transform.position = targetPosition;
            }
        }

        if (!reachedTarget) {
            shipScript.Propeller(true);
            shipScript.Turn(shipScript.TurnCorrection(WishToGoDirection()) < 0);
        }

        if ((targetPosition - gameObject.transform.position).magnitude < distanceBeforeTargetReached) {
            reachedTarget = true;
            if (activeMoveMark != null) {
                Destroy(activeMoveMark);
                activeMoveMark = null;
            }
        }
    }

    Vector2 GetVectorPlayerToTarget() {
        return targetPosition - gameObject.transform.position;
    }

    Vector2 WishToGoDirection() {
        Vector2 distance = GetVectorPlayerToTarget();
        Vector2 directVector = distance.normalized;
        return directVector;
    }
}
