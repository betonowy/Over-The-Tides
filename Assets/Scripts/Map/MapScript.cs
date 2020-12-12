using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    public GameObject map;
    private bool flag;

    void Update() {
        if(Input.GetKeyDown(KeyCode.M) ) {
            if (map.activeSelf == true) {
                map.SetActive(false);
            }
            else {
                map.SetActive(true);
            }
        }   
    }
}
