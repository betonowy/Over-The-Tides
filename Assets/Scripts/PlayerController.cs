using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;
//using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    Rigidbody2D rb;

    private Vector3 targetPosition;
    private float targetDistance;

    public float turnSpeed;
    public float moveSpeed;

    public GameObject projectile;
    private float timeBtwShoots;
    public float startTimeBtwShoots;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();

        timeBtwShoots = startTimeBtwShoots;
    }

    private void Update() {
        mouseMovement();
        shooting();
    }

    private void shooting() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            if (timeBtwShoots <= 0) {
                Instantiate(projectile, transform.position, Quaternion.identity);
                timeBtwShoots = startTimeBtwShoots;
            }
            else {
                timeBtwShoots -= Time.deltaTime;
            }
        }
    }


    private void mouseMovement() {
        if (Input.GetMouseButtonDown(0))
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //else
        //targetPosition = transform.position;
        //Debug.Log(targetPosition.ToString());
        //Debug.Log(transform.position.ToString());

        targetDistance = Vector3.Distance(targetPosition.normalized, transform.position.normalized);

        turnSpeed = 0.03f * targetDistance;
        moveSpeed = 50f * targetDistance;

        Vector2 temp1 = new Vector2(targetPosition.x, targetPosition.y);
        Vector2 temp2 = new Vector2(transform.position.x, transform.position.y);
        
        if (Vector3.Distance(temp1, temp2) > 0.1f)
            rb.AddForce(transform.up * moveSpeed * Time.deltaTime);

        var newRotation = Quaternion.LookRotation(transform.position - targetPosition, Vector3.forward);
        newRotation.x = 0f;
        newRotation.y = 0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * turnSpeed);
    }


}