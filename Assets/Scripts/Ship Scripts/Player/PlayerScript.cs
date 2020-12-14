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
    public GameObject shipToSpawn;
    public GameObject sailor;

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
                GameObject plank = Instantiate(plankToPick, gameObject.transform.position, gameObject.transform.rotation);
                plank.transform.position = gameObject.transform.position;
            }
            return;
        }
        else if (item == "ship") {
            for (int i = 0; i < amount; i++) {
               
                GameObject[] allGM = GameObject.FindGameObjectsWithTag("Ship");
                Vector3 v = new Vector3(transform.position.x + 40, transform.position.y + 40, 0);
                while(!CheckSpace(allGM, v, 10)) {
                    v = new Vector3(transform.position.x + UnityEngine.Random.Range(40, 90), transform.position.y + UnityEngine.Random.Range(40, 90), 0);
                }
                GameObject ship = Instantiate(shipToSpawn, v, gameObject.transform.rotation);
               // ship.transform.position = gameObject.transform.position;
            }
            return;
        }
        else if (item == "heal") {
            for (int i = 0; i < amount; i++) {
                GameObject[] allGM = GameObject.FindGameObjectsWithTag("Ship");
                for(int j = 0; j < allGM.Length; j++) {
                    if (allGM[j].GetComponent<ShipScript>().team == ShipScript.teamEnum.teamBlue)
                        allGM[j].GetComponent<ShipScript>().HealFullHealth();
                }
            }
            return;
        }
        else if (item == "sailor") {
            for (int i = 0; i < amount; i++) {
                GameObject obj = Instantiate(sailor, gameObject.transform.position, gameObject.transform.rotation);
                obj.transform.parent = gameObject.transform;
                obj.transform.position = gameObject.transform.position;
                obj.transform.GetComponent<SailorScript>().shipPosition = new Vector2(0, -0.8f);
                obj.transform.GetComponent<SailorScript>().targetShipPosition = new Vector2(0, -0.8f);
            }
            return;
        }
    }

    private bool CheckSpace(GameObject[] objects, Vector2 position, float radius) {
        foreach (GameObject obj in objects) {
            if ((position - (Vector2)obj.transform.position).magnitude < radius) {
                Debug.Log("Spawn space not OK");
                return false;
            }
        }
        Debug.Log("Spawn space OK");
        return true;
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
