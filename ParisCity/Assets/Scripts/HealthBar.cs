using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider HealthSlider;
    public int currentHealth = 0;
    public Image FillImage;

    void Start()
    {
        currentHealth = (int)HealthSlider.maxValue;
        //  HealthSlider.value = HealthSlider.maxValue / 2; 50
    }

    public void ReduceHealth(int amount)
    {
        currentHealth -= amount;
        HealthSlider.value = currentHealth;

        if (currentHealth < HealthSlider.maxValue / 4)
        {
            FillImage.color = Color.red;
        }
        else if (currentHealth < HealthSlider.maxValue / 2)
        {
            FillImage.color = Color.yellow;

        }
    }


}
