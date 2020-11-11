using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShwipwreckScript : MonoBehaviour
{

    BoxCollider2D boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ship")
            Destroy(gameObject);
    }
}
