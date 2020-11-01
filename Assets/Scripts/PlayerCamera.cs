﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Camera cam;
    static bool check = false;
    public GameObject PlayerBoat;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.enabled = false;
    }

    private void LateUpdate() {
        if (check) {
            cam.enabled = true;
            PlayerBoat.SendMessage("setOrderMode", true);
        }
        else {
            cam.enabled = false;
            PlayerBoat.SendMessage("setOrderMode", false);
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
}
