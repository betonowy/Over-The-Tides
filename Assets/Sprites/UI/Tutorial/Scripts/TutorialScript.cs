using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    public Sprite[] popUps;

    [SerializeField]
    private GameObject obj;

    [SerializeField]
    private GameObject Image;

    private int popUpIndex = 0;
    private bool flag = true;

    private bool playerScriptCaptured = false;
    private PlayerScript ps;

    private void Start() {
        Image.GetComponent<Image>().sprite = popUps[0];
    }

    public void Next() {
        popUpIndex++;
        if (popUpIndex > 10)
            popUpIndex = 10;
        Debug.Log(popUpIndex);
        if(popUpIndex <= 10)
            Image.GetComponent<Image>().sprite = popUps[popUpIndex];
    }

    public void Back() {
        popUpIndex--;
        if (popUpIndex < 0)
            popUpIndex = 0;
        if(popUpIndex >= 0)
            Image.GetComponent<Image>().sprite = popUps[popUpIndex];
    }

    public void Hide() {
        if (flag) {
            obj.SetActive(false);
            flag = false;
        } else {
            obj.SetActive(true);
            flag = true;
        }
    }
    private void Update() {
        if (!playerScriptCaptured) {
            ps = FindObjectOfType<PlayerScript>();
            if (ps != null) {
                playerScriptCaptured = true;
                Rect[] newRects = new Rect[2];
                Rect newRect = new Rect();
                float ratio = (float)Screen.height / Screen.width;
                // tutorial window
                newRect.width = 480f / Screen.width;
                newRect.height = 500f / Screen.height;
                newRect.x = 30f / Screen.width;
                newRect.y = 0.5f - newRect.height / 2;
                newRects[0] = newRect;

                // hide button
                newRect.width = 100f / Screen.width;
                newRect.height = 50f / Screen.height;
                newRect.x = 20f / Screen.width;
                newRect.y = 20f / Screen.height;
                newRects[1] = newRect;

                ps.deniedRects = newRects;
                ps.rectDenied = !obj.activeSelf;
            }
        }
    }

    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
}
