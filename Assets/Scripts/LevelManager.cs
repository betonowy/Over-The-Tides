using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public PlayerCamera playerCamera;
    public CameraScript mainCamera;
    public ProjectPlayer projectPlayer;

    private bool lockCameraChange = false;

    // Start is called before the first frame update
    void Start() {
        playerCamera = FindObjectOfType<PlayerCamera>();
        mainCamera = FindObjectOfType<CameraScript>();
        projectPlayer = FindObjectOfType<ProjectPlayer>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !lockCameraChange) {
            mainCamera.onCheck();
            playerCamera.offCheck();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && !lockCameraChange) {
            mainCamera.offCheck();
            playerCamera.onCheck();
        }
        if (Input.GetKey(KeyCode.R)) {
            mainCamera.increaseZoom();
        }
        if (Input.GetKey(KeyCode.F)) {
            mainCamera.decreaseZoom();
        }
        /*if (Input.GetKeyDown(KeyCode.Q)) {
            projectPlayer.shotLeft();
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            projectPlayer.shotRight();
        }*/
    }

    void OnPlayerDeath() {
        lockCameraChange = true;
        mainCamera.onCheck();
        playerCamera.offCheck();
    }
}
