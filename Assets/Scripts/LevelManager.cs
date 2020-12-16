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

    private bool lockCameraChange = false;
    public bool nextSceneAfterTDM = false;

    private HordeMan hordeManager;

    public ItemDatabaseObject database;

    // Start is called before the first frame update
    void Start() {
        playerCamera = Instantiate(playerCameraPrefab);
        GameObject player;
        if ((player = GameObject.Find("playerBoatFFA")) != null) {
            playerCamera.transform.SetParent(player.transform);
            player.GetComponent<ShipScript>().MakeLeader();
        } else if ((player = GameObject.Find("playerBoatBlue")) != null) {
            playerCamera.transform.SetParent(player.transform);
            player.GetComponent<ShipScript>().MakeLeader();
        } else if ((player = GameObject.Find("playerBoatRed")) != null) {
            playerCamera.transform.SetParent(player.transform);
            player.GetComponent<ShipScript>().MakeLeader();
        }
        mainCamera = Instantiate(mainCameraPrefab);
        hordeManager = gameObject.GetComponent<HordeMan>();
    }

    // Update is called once per frame
    void Update() {
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

        if (Input.GetKeyDown(KeyCode.P)) {
            foreach (GameObject s in ship) {
                if (s.GetComponent<ShipScript>().team == ShipScript.teamEnum.teamRed)
                    s.SendMessage("Damage", 100);
            }
        }

        if (nextSceneAfterTDM && GamemodeConditions() && ((!reds || !blues) && !(!reds && !blues) || ship.Length <= 1)) {
            OnGameEnd();
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
