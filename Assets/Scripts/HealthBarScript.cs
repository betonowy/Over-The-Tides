using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider slider;

    public void setMaxHealth(int health) {
        slider.maxValue = health;
        slider.value = health;
    }
    
    public void setHealth(int health) {
        slider.value = health;
        for (int i = 0; i > transform.childCount; i++) {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.name == "Fill") {
                child.GetComponent<RectTransform>().localScale = new Vector3(slider.value / slider.maxValue, 1, 1);
            }
        }
    }
}
