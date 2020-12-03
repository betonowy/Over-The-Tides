using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading;

public class LevelManager : MonoBehaviour {
    public PlayerCamera playerCameraPrefab;
    public CameraScript mainCameraPrefab;

    private PlayerCamera playerCamera;
    private CameraScript mainCamera;

    public TimerScript timerScript;
    private bool lockCameraChange = false;
    public bool nextSceneAfterTDM = false;

    private HordeMan hordeManager;

    public ItemDatabaseObject database;

    public Quest quest;
    
    //delete later
    public PlayerScript player;
    public GameObject text;

    // Start is called before the first frame update
    void Start() {
        playerCamera = Instantiate(playerCameraPrefab);
        if (GameObject.Find("playerBoatFFA") != null) {
            playerCamera.transform.SetParent(GameObject.Find("playerBoatFFA").transform);
        } else if (GameObject.Find("playerBoatBlue") != null) {
            playerCamera.transform.SetParent(GameObject.Find("playerBoatBlue").transform);
        } else if (GameObject.Find("playerBoatRed") != null) {
            playerCamera.transform.SetParent(GameObject.Find("playerBoatRed").transform);
        }
        mainCamera = Instantiate(mainCameraPrefab);
        hordeManager = gameObject.GetComponent<HordeMan>();


    }

    // Update is called once per frame
    void Update() {
        Time.timeScale = timerScript.GetTimeModifier();
        if (Input.GetKeyDown(KeyCode.Alpha1) && !lockCameraChange) {
            mainCamera.setFreeMove(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && !lockCameraChange) {
            playerCamera.toggleCheck();
        }
        if (Input.mouseScrollDelta.y > 0) {
            mainCamera.increaseZoom();
        }
        if (Input.mouseScrollDelta.y < 0) {
            mainCamera.decreaseZoom();
        }

        //delete later
        if (Input.GetKey(KeyCode.R))
            text.SetActive(false);
        //

        GameObject[] ship = GameObject.FindGameObjectsWithTag("Ship");
        bool reds = false;
        bool blues = false;
        foreach (GameObject s in ship) {
            if (s.GetComponent<ShipScript>().team == ShipScript.teamEnum.teamRed) {
                reds = true;
            } else if (s.GetComponent<ShipScript>().team == ShipScript.teamEnum.teamBlue) {
                blues = true;
            }
        }

        if (nextSceneAfterTDM && GamemodeConditions() && ((!reds || !blues ) && !(!reds && !blues) || ship.Length <= 1 )) {
            OnGameEnd();
        }
    }
    //delete later
    public void SetQuest(Quest playerQuest) {
        quest = playerQuest;
    }

    void OnEnemyDeath(ShipScript ship) {
        if (ship == null)
            Debug.Log("nulls");
        if (quest.isActive) {
            if (ship.GetComponent<ShipScript>().team == ShipScript.teamEnum.teamRed) {
                quest.goal.EnemyKilled();
            }
            if(quest.goal.IsReached()) {
                //delete later
                player.createCannon();
                quest.Complete();
                text.SetActive(true);
            }
        }
    }

    void OnPlayerDeath() {
        lockCameraChange = true;
        mainCamera.onCheck();
        playerCamera.offCheck();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    bool GamemodeConditions() {
        if (hordeManager) {
            return hordeManager.EndGameCondition();
        }
        return true;
    }

    void OnGameEnd() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
