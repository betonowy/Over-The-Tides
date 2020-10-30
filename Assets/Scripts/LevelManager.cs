using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public PlayerCamera playerCamera;
    public CameraScript mainCamera;

    // Start is called before the first frame update
    void Start() {
        playerCamera = FindObjectOfType<PlayerCamera>();
        mainCamera = FindObjectOfType<CameraScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            mainCamera.onCheck();
            playerCamera.offCheck();
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            mainCamera.offCheck();
            playerCamera.onCheck();
        }
    }
}
