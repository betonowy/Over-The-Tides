using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IslandScript : MonoBehaviour {
    
    public GameObject islandUI;
    public GameObject islandWindow;
    public GameObject islandLog;
    public GameObject acceptBtn;
    public QuestGiverScript questGiver;

    private PlayerScript player;

    public bool isCompleted = false;
    private bool flag = true;

    public string desc;
    public string rewa;

    void Start() {        
        islandUI = GameObject.Find("IslandUI");
        player = GameObject.Find("playerBoatBlue").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.J) && islandWindow.activeSelf == true) {
            islandWindow.transform.Find("IslandLog").gameObject.SetActive(true);
            islandLog = GameObject.Find("IslandLog");
            GameObject.Find("playerBoatBlue").GetComponent<PlayerScript>().SendMessage("AllowMovement", false);

        }

        if (isCompleted && flag) {
            player.GenerateReward(rewa);
            flag = false;
        }

    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.name == "playerBoatBlue") {
            islandUI.transform.Find("IslandWindow").gameObject.SetActive(true);
            islandWindow = GameObject.Find("IslandWindow");
            islandWindow.SetActive(true);
            questGiver.SetIsland(this);
            questGiver.SetIslandText(this);
            
        }
    }

    public void OnTriggerExit2D(Collider2D collision) {
        if (collision.name == "playerBoatBlue") {
            GameObject.Find("playerBoatBlue").GetComponent<PlayerScript>().SendMessage("AllowMovement", true);
            islandWindow.SetActive(false);
            islandLog.SetActive(false);
        }
    }

    public void Accept() {
        isCompleted = true;
        islandLog.SetActive(false);
        GameObject.Find("playerBoatBlue").GetComponent<PlayerScript>().SendMessage("AllowMovement", true);
    }

    public void Quit() {
        GameObject.Find("playerBoatBlue").GetComponent<PlayerScript>().SendMessage("AllowMovement", true);
        islandLog.SetActive(false);
    }
}
