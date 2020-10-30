using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    Camera cam;
    bool check = false;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.enabled = false;
    }

    private void LateUpdate() {
        if (check) {
            cam.enabled = true;
        }
        else {
            cam.enabled = false;
        }
    }

    public void onCheck() {
        check = true;
    }

    public void offCheck() {
        check = false;
    }
}
