using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    public PlayerCamera playerCameraPrefab;
    public CameraScript mainCameraPrefab;

    private PlayerCamera playerCamera;
    private CameraScript mainCamera;

    private bool lockCameraChange = false;

    public bool nextSceneAfterTDM = false;

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
        if (nextSceneAfterTDM && ((!reds || !blues ) && !(!reds && !blues) || ship.Length <= 1 )) {
            OnGameEnd();
        }
    }

    void OnPlayerDeath() {
        lockCameraChange = true;
        mainCamera.onCheck();
        playerCamera.offCheck();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void OnGameEnd() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
