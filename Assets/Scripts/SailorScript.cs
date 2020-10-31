using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailorScript : MonoBehaviour
{
    Rigidbody2D rBody;
    private Vector3 positionOnShip;
    private Quaternion sailorRotation;
    PlayerController ship;
    private float alpha;
    private float newX;
    private float newY;
    public float ship_x;
    public float ship_y;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        ship = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        stayOnShip();
    }

    private void stayOnShip()
    {
        alpha = -ship.gameObject.GetComponent<Rigidbody2D>().rotation * Mathf.Deg2Rad;
        newX = -Mathf.Sin(alpha) * ship_x + Mathf.Cos(alpha) * ship_y;
        newY = Mathf.Cos(alpha) * ship_y + Mathf.Sin(alpha) * ship_x;
        positionOnShip.x = newX;
        positionOnShip.y = newY;
        gameObject.transform.position = ship.gameObject.transform.position + positionOnShip;
        rBody.rotation = ship.gameObject.GetComponent<Rigidbody2D>().rotation;
    }
}
