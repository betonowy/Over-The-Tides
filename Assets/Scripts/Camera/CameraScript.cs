using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Camera))]
public class CameraScript : MonoBehaviour {

    public Vector3 offset;
    public float smoothTime = .2f;

    public float minZoom = 200f;
    public float maxZoom = 10f;
    public float zoom = 20f;
    public float zoomSpeed = 0.01f;
    public float moveSpeed = 0.05f;

    private Vector3 velocity;
    private Camera cam;
    private bool freeMove = false;

    public float minSizeY = 3f;

    private bool check = true;

    public float playerViewWidth;
    public float playerViewHeight;

    private GameObject playerObject;

    public GameObject quad;

    private float height = 1;
    private float width;

    private void Start() {
        cam = GetComponent<Camera>();
        cam.enabled = true;

        width = height * Camera.main.aspect;

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                var newQuad = Instantiate(quad, new Vector3(x - width / 2f, y - height / 2f, 50), Quaternion.identity, gameObject.transform);
            }
        }

        if (GameObject.Find("playerBoatFFA") != null) {
            playerObject = GameObject.Find("playerBoatFFA");
        } else if(GameObject.Find("playerBoatBlue") != null) {
            playerObject = GameObject.Find("playerBoatBlue");
        } else if (GameObject.Find("playerBoatRed") != null) {
            playerObject = GameObject.Find("playerBoatRed");
        }
    }

    private void Update() {
        if (check) {
            cam.enabled = true;
        } else {
            //cam.enabled = false;
        }

        if (cam.enabled == true) {
            Move();
            Zoom();
        }

        if (Input.GetKeyDown(KeyCode.Z)) {
            freeMove = !freeMove;
        }
    }

    private void Move() {
        if (!freeMove) {
            try {
                Vector3 centerPoint = playerObject.transform.position;
                centerPoint.z = -10;
                transform.position = Vector3.SmoothDamp(transform.position, centerPoint, ref velocity, smoothTime);
            } catch {
                freeMove = true;
            }
        } else {
            Vector3 centerPoint = transform.position;
            if (Input.GetKey(KeyCode.W)) {
                centerPoint.y += moveSpeed * zoom;
            }
            if (Input.GetKey(KeyCode.S)) {
                centerPoint.y -= moveSpeed * zoom;
            }
            if (Input.GetKey(KeyCode.A)) {
                centerPoint.x -= moveSpeed * zoom;
            }
            if (Input.GetKey(KeyCode.D)) {
                centerPoint.x += moveSpeed * zoom;
            }
            transform.position = centerPoint;
        }
    }

    private void Zoom() {
        if (minZoom > zoom) zoom = minZoom;
        else if (maxZoom < zoom) zoom = maxZoom;
        cam.orthographicSize = zoom;
    }

    public void onCheck() {
        check = true;
    }

    public void offCheck() {
        check = false;
    }

    public void increaseZoom() {
        zoom *= 1 + zoomSpeed;
    }

    public void decreaseZoom() {
        zoom *= 1 - zoomSpeed;
    }
}
