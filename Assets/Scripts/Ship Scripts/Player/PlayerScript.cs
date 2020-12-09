using System;
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

    public InventoryObject inventory;
    
    public GameObject cannonToPick;
    public GameObject goldToPick;
    public GameObject plankToPick;

    public bool rectDenied;
    public Rect[] deniedRects;

    private bool allowMove = true;

    Vector3 pos;

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
        healthBar.SendMessage("setHealth", shipScript.shipLife);
        Movement();
        Shooting();
    }

    void Shooting() {
        if (Input.GetKey(KeyCode.Q) && shipScript.GetPaddleObject().getNodeScript().ReadyCrewCount() == 0)
            shipScript.ShootLeft();
        if (Input.GetKey(KeyCode.E) && shipScript.GetPaddleObject().getNodeScript().ReadyCrewCount() == 0)
            shipScript.ShootRight();
    }

    private void Movement() {
        if (Input.GetMouseButtonDown(0) && allowMove) {
            Vector2 mousePos = Input.mousePosition;

            bool allow = true;

            if (rectDenied) {
                if (MouseInDeniedRect())
                    allow = false;
            }

            if (allow) {
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

    private bool MouseInDeniedRect() {
        Vector2 viewport = Camera.allCameras[0].ScreenToViewportPoint(Input.mousePosition);
        foreach (Rect dr in deniedRects) {
            if (dr.Contains(viewport))
                return true;
        }
        return false;
    }

    Vector2 GetVectorPlayerToTarget() {
        return targetPosition - gameObject.transform.position;
    }

    Vector2 WishToGoDirection() {
        Vector2 distance = GetVectorPlayerToTarget();
        Vector2 directVector = distance.normalized;
        return directVector;
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        var item = collision.GetComponent<GroundItem>();
        if(item) {
            inventory.AddItem(new Item(item.item), 1);
            Destroy(collision.gameObject);
        }
    }

    public void AddItems(Item item, int amount) {
        inventory.AddItem(item, amount);
    }

    public void createCannon() {
        GameObject cnn = Instantiate(cannonToPick, gameObject.transform.position, gameObject.transform.rotation);
        cnn.transform.position = gameObject.transform.position;
    }

    private void CreateItems(string item, int amount) {
        if (item == "cannon") {
            for(int i=0; i<amount; i++) {
                GameObject cnn = Instantiate(cannonToPick, gameObject.transform.position, gameObject.transform.rotation);
                cnn.transform.position = gameObject.transform.position;
            }
            return;
        } else if(item == "gold") {
            for (int i = 0; i < amount; i++) {
                GameObject gold = Instantiate(goldToPick, gameObject.transform.position, gameObject.transform.rotation);
                gold.transform.position = gameObject.transform.position;
            }
            return;
        } else if (item == "plank") {
            for (int i = 0; i < amount; i++) {
                Debug.Log(amount);
                GameObject plank = Instantiate(plankToPick, gameObject.transform.position, gameObject.transform.rotation);
                plank.transform.position = gameObject.transform.position;
            }
            return;
        }
    }

    public void GenerateReward(string s) {
        string[] words = s.Split(' ');
        CreateItems(words[1], Int16.Parse(words[2]));
    }

    private void OnApplicationQuit() {
        inventory.Container.Items = new InventorySlot[24];
    }

    public void allowMovement(bool var) {
        allowMove = var;
    }
}
