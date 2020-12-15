using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    public GameObject map;
    public GameObject legend;
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

        if (Input.GetKeyDown(KeyCode.L) && map.activeSelf == true) {
            if (legend.activeSelf == true) {
                legend.SetActive(false);
            }
            else {
                legend.SetActive(true);
            }
        }

    }
}
