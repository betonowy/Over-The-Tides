using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    public Camera cam;
    static bool check = false;
    public float widthMod = 0.15f;
    public float offsetY = 1;

    void Start() {
        cam = GetComponent<Camera>();
        cam.enabled = false;
        Vector3 tempPos = transform.position;
        tempPos.y += offsetY;
        transform.position = tempPos;
    }

    private void Update() {
        try {
            var temp = cam.rect;
            temp.width = widthMod / ((float)Screen.width / Screen.height);
            temp.x = 1 - temp.width;
            cam.rect = temp;
            temp = Camera.allCameras[0].rect;

            if (check) {
                cam.enabled = true;
                temp.width = 1 - cam.rect.width;
            } else {
                cam.enabled = false;
                temp.width = 1;
            }

            Camera.allCameras[0].rect = temp;
        } catch {
            offCheck();
        }
    }

    public bool getCheck() {
        return check;
    }

    public void onCheck() {
        check = true;
    }

    public void offCheck() {
        check = false;
    }
    public void toggleCheck() {
        check = !check;
    }
}
