using UnityEngine;

public class MinimapScript : MonoBehaviour {

    private GameObject player;
    private float x, y;

    void Start() {
        player = GameObject.Find("playerBoatBlue");
        x = gameObject.GetComponent<Transform>().position.x;
        y = gameObject.GetComponent<Transform>().position.y;
    }

    void Update() {
        ChangePos();
    }

    void ChangePos() {
        gameObject.GetComponent<Transform>().position = new Vector3(x + player.transform.position.x, y + player.transform.position.y, -10);
    }
}
