using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShwipwreckScript : MonoBehaviour
{
    public GameObject cannonToPick;
    public GameObject GoldToPick;
    public GameObject plankToPick;
    Vector3 pos;

    BoxCollider2D boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        match_gold();
        match_plank();
        pos = boxCollider.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "playerBoatBlue")
            Destroy(gameObject);
    }
    
    private void match_plank() {
        for (int i = 0; i < Random.Range(5, 15); i++) {
            GameObject obj = Instantiate(plankToPick, gameObject.transform.position, gameObject.transform.rotation);
            obj.GetComponent<BoxCollider2D>().size = boxCollider.size;
            obj.GetComponent<BoxCollider2D>().transform.Translate(pos);
            obj.transform.position = gameObject.transform.position;
        }
    }


    private void match_cannon()
    {
        GameObject cnn = Instantiate(cannonToPick, gameObject.transform.position, gameObject.transform.rotation);
        cnn.GetComponent<BoxCollider2D>().size = boxCollider.size;
        cnn.GetComponent<BoxCollider2D>().transform.Translate(pos);
        cnn.transform.position = gameObject.transform.position;
    }

    private void match_gold()
    {
        for(int i = 0; i< Random.Range(10, 20); i++) {  
            GameObject gld = Instantiate(GoldToPick, gameObject.transform.position, gameObject.transform.rotation);
            gld.GetComponent<BoxCollider2D>().size = boxCollider.size;
            gld.GetComponent<BoxCollider2D>().transform.Translate(pos);
            gld.transform.position = gameObject.transform.position;
        }
    }
}
