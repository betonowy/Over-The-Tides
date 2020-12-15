using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    public GameObject map;
    public GameObject legend;
    private bool flag;
    public GameObject marker;
    public MarkerScript markerScript;
    public bool markerFlag = false;

    void Update() {
        if(Input.GetKeyDown(KeyCode.M) ) {
            if (map.activeSelf == true) {
                if (markerFlag == true) {
                    if (markerScript.flag == false)
                        markerScript.StopBlinking();
                    marker.SetActive(false);
                }
                map.SetActive(false);
            }
            else {
                map.SetActive(true);
                if (markerFlag == true) {
                    marker.SetActive(true);
                    if (markerScript.flag == true)
                        markerScript.StartBlinking();
                }
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
