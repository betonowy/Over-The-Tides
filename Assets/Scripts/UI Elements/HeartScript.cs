using UnityEngine;
using UnityEngine.UI;

public class HeartScript : MonoBehaviour
{
    private GameObject health;
    public Sprite hSprite1;
    public Sprite hSprite2;
    public Sprite hSprite3;
    public Sprite hSprite4;

    void Start()
    {
        health = GameObject.Find("Health bar");
    }

    void Update()
    {
        if (health.GetComponent<Slider>().value <= 75 && health.GetComponent<Slider>().value > 40) {
            gameObject.GetComponent<Image>().sprite = hSprite2;
        } else if (health.GetComponent<Slider>().value <= 40 && health.GetComponent<Slider>().value > 10) {
            gameObject.GetComponent<Image>().sprite = hSprite3;
        } else if (health.GetComponent<Slider>().value <= 10) {
            gameObject.GetComponent<Image>().sprite = hSprite4;
        } else {
            gameObject.GetComponent<Image>().sprite = hSprite1;
        }
    }
}
