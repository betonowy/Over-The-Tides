using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBox : MonoBehaviour
{

    [SerializeField] private RectTransform selectSquareImage;

    [SerializeField] public Camera camera;

    private Vector3 startPos;
    private Vector3 endPos;

    // Start is called before the first frame update
    void Start() {
        selectSquareImage.gameObject.SetActive(false);
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update() {
        if (camera.enabled) {
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity)) {
                    startPos = hit.point;

                }
            }

            if (Input.GetMouseButtonUp(0)) {
                selectSquareImage.gameObject.SetActive(false);
            }

            if (Input.GetMouseButton(0)) {
                if (!selectSquareImage.gameObject.activeInHierarchy) {
                    selectSquareImage.gameObject.SetActive(true);
                }
                endPos = Input.mousePosition;

                Vector3 squareStart = camera.WorldToScreenPoint(startPos);
                squareStart.z = 0f;

                Vector3 center = (squareStart + endPos) / 2f;

                selectSquareImage.position = center;

                float sizeX = Mathf.Abs(squareStart.x - endPos.x);
                float sizeY = Mathf.Abs(squareStart.y - endPos.y);

                selectSquareImage.sizeDelta = new Vector2(sizeX, sizeY);

            }
        }
    }
}
