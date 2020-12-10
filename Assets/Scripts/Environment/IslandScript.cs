using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IslandScript : MonoBehaviour {
    
    public GameObject islandUI;
    public GameObject islandWindow;
    public GameObject islandLog;
    public GameObject acceptBtn;

    private PlayerScript player;

    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI rewardText;

    private bool isCompleted = false;
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

            descriptionText = islandLog.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            rewardText = islandLog.transform.Find("Reward").GetComponent<TextMeshProUGUI>();

            descriptionText.text = desc;
            rewardText.text = rewa;

        }

        if (isCompleted && flag) {
            player.GenerateReward(rewardText.text);
            flag = false;
        }

    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.name == "playerBoatBlue") {
            islandUI.transform.Find("IslandWindow").gameObject.SetActive(true);
            islandWindow = GameObject.Find("IslandWindow");
        }
    }

    public void OnTriggerExit2D(Collider2D collision) {
        if (collision.name == "playerBoatBlue") {
            islandWindow.SetActive(false);
            islandLog.SetActive(false);
        }
    }

    public void Accept() {
        isCompleted = true;
        islandLog.SetActive(false);
    }

    public void Quit() {
        islandLog.SetActive(false);
    }
}
