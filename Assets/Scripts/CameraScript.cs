using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Camera))]
public class CameraScript : MonoBehaviour {
    public List<Transform> targets;

    public Vector3 offset;
    public float smoothTime = .2f;

    public float minZoom = 200f;
    public float maxZoom = 10f;
    public float zoomLimiter = 300f;

    private Vector3 velocity;
    private Camera cam;

    public float minSizeY = 3f;

    private bool check = true;

    public float playerViewWidth;
    public float playerViewHeight;


    public GameObject quad;

    private float height = 1;
    private float width;

    private void Start() {
        cam = GetComponent<Camera>();
        cam.enabled = true;

        width = height * Camera.main.aspect;
        
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                var newQuad = Instantiate(quad, new Vector3(x - width / 2f, y - height / 2f, 50), Quaternion.identity);
                newQuad.transform.parent = gameObject.transform;
            }
        }
    }

    private void LateUpdate() {
        if (targets.Count == 0)
            return;

        if (check) {
            cam.enabled = true;
        } else {
            cam.enabled = false;
        }

        if (cam.enabled == true) {
            Move();
            Zoom();
        }

    }

    private void Move() {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);

    }

    private void Zoom() {
        //float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatesDistance());
        //cam.orthographicSize = Mathf.Max(height, camSizeX * Screen.height / Screen.width, minSizeY);
        //Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);

        // float width = Mathf.Abs(player.position.x - enemy.position.x) * 0.8f;
        // float height = Mathf.Abs(player.position.y - enemy.position.y) * 0.8f;

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++) {
            bounds.Encapsulate(targets[i].position);
        }

        //computing the size
        float minSizeX = minSizeY * Screen.width / Screen.height;
        float camSizeX = GetGreatesDistance() * 0.8f;
        cam.orthographicSize = Mathf.Max(bounds.size.x * 0.8f, camSizeX * Screen.height / Screen.width, bounds.size.y);
    }

    private float GetGreatesDistance() {

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++) {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.size.x;
    }
    Vector3 GetCenterPoint() {
        if (targets.Count == 1)
            return targets[0].position;

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++) {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }

    private Transform GetPlayerPos() {
        int index = 0;
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++) {
            bounds.Encapsulate(targets[i].position);
            if (targets[i].CompareTag("Player")) {
                index = i;
                break;
            }
        }
        return targets[index];
    }

    private void zoomPlayer() {
        Transform player = GetPlayerPos();

        cam.orthographicSize = 5.0f;
        cam.rect = new Rect(player.position.x, player.position.y, playerViewWidth, playerViewHeight);

    }

    public void onCheck() {
        check = true;
    }

    public void offCheck() {
        check = false;
    }
}
