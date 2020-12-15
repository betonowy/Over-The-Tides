using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerScript : MonoBehaviour
{
    public GameObject marker;
    private Image image;
    public bool flag = false;

    //private void Update() {
    //    if(flag) {
    //        StartBlinking();
    //    } else {
    //        StopBlinking();
    //    }

    //}

    IEnumerator Blink() {
        while (true) {
            switch (marker.GetComponent<Image>().color.a.ToString()) {
                case "0":
                    marker.GetComponent<Image>().color = new Color(marker.GetComponent<Image>().color.r, marker.GetComponent<Image>().color.g, marker.GetComponent<Image>().color.b, 1);
                    yield return new WaitForSeconds(0.5f);
                    break;
                case "1":
                    marker.GetComponent<Image>().color = new Color(marker.GetComponent<Image>().color.r, marker.GetComponent<Image>().color.g, marker.GetComponent<Image>().color.b, 0);
                    yield return new WaitForSeconds(0.5f);
                    break;
            }
        }
    }

    public void StartBlinking() {
        StopAllCoroutines();
        StartCoroutine("Blink");
    }

    public void StopBlinking() {
        StopAllCoroutines();
    }
}
