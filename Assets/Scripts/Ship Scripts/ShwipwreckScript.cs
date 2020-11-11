using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShwipwreckScript : MonoBehaviour
{
    public GameObject cannonToPick;
    public GameObject GoldToPick;
    Vector3 pos;

    BoxCollider2D boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        match_cannon();
        match_gold();
        pos = boxCollider.transform.position;
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

    private void match_cannon()
    {
        GameObject cnn = Instantiate(cannonToPick);
        //cnn.transform.SetParent(gameObject.transform);
        //cnn.GetComponent<BoxCollider2D>().size = boxCollider.size;
        //cnn.GetComponent<BoxCollider2D>().transform.Translate(pos);
        cnn.transform.position = gameObject.transform.position;
    }

    private void match_gold()
    {
        GameObject gld = Instantiate(GoldToPick);
        //gld.transform.SetParent(gameObject.transform);
        //gld.GetComponent<BoxCollider2D>().size = boxCollider.size;
        //gld.GetComponent<BoxCollider2D>().transform.Translate(pos);
        gld.transform.position = gameObject.transform.position;
    }
}
