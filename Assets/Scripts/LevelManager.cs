using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public PlayerCamera playerCameraPrefab;
    public CameraScript mainCameraPrefab;

    private PlayerCamera playerCamera;
    private CameraScript mainCamera;

    private bool lockCameraChange = false;

    // Start is called before the first frame update
    void Start() {
        playerCamera = Instantiate(playerCameraPrefab);
        playerCamera.transform.SetParent(GameObject.Find("playerBoat").transform);
        mainCamera = Instantiate(mainCameraPrefab);
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
    }

    void OnPlayerDeath() {
        lockCameraChange = true;
        mainCamera.onCheck();
        playerCamera.offCheck();
    }
}
