using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatIconScript : MonoBehaviour
{
    private GameObject player;
    private float x, y;

    void Start() {
        player = GameObject.Find("playerBoatBlue");
        x = gameObject.GetComponent<RectTransform>().position.x;
        y = gameObject.GetComponent<RectTransform>().position.y;
        //gameObject.GetComponent<RectTransform>().position = new Vector3(420, 300, 0);
        //Debug.Log(gameObject.GetComponent<RectTransform>().position);
    }

    private void Update() {
        gameObject.GetComponent<RectTransform>().position = new Vector3(x + player.transform.position.x * 0.2f, y + player.transform.position.y * 0.2f, 0);
        gameObject.GetComponent<RectTransform>().rotation = player.transform.rotation;
    }

}
