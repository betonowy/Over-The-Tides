using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class SelectBox : MonoBehaviour {

    [SerializeField] public RectTransform selectSquareImage;
    [SerializeField] public Camera cam;

    private Vector3 startPos;
    private Vector3 endPos;

    // Start is called before the first frame update
    void Start() {
        selectSquareImage.gameObject.SetActive(false);
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update() {
        if (cam.enabled) {
            Vector2 mousePoint = Input.mousePosition;

            if (Input.GetMouseButtonDown(0) && selectBoxController.mouseInsideCameraViewport()) {
                startPos = mousePoint;
                selectSquareImage.gameObject.SetActive(true);
            }

            if (Input.GetMouseButtonUp(0)) {
                selectSquareImage.gameObject.SetActive(false);
            }

            if (Input.GetMouseButton(0) && selectBoxController.mouseInsideCameraViewport()){
                endPos = mousePoint;

                selectSquareImage.position = (startPos + endPos) / 2f;

                Vector2 size = endPos - startPos;
                if (size.x < 0) size.x = -size.x;
                if (size.y < 0) size.y = -size.y;

                selectSquareImage.sizeDelta = size;
            }
        }
    }
}
