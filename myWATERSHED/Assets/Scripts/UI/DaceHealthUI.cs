using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaceHealthUI : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    public void SetMaxHealth(int health) {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health) {
        slider.value = health;

        fill.color = new Color((1f * (slider.value/slider.maxValue)), (1f * (1f - (slider.value / slider.maxValue))), 0f);
    }
}
