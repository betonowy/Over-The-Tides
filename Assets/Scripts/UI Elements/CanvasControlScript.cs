using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasControlScript : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasInventory;
    bool inv = false;

    private void Start() {
        Hide();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            if (!inv)
                Show();
            else
                Hide();
        }

    }

    void Hide() {
        canvasInventory.alpha = 0f;
        canvasInventory.blocksRaycasts = false;
        inv = false;
    }

    void Show() {
        canvasInventory.alpha = 1f;
        canvasInventory.blocksRaycasts = true;
        inv = true;
    }

}
