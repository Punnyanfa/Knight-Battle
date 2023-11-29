using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthBarText;

    Damageable playerDamageable;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerDamageable = player.GetComponent<Damageable>();
    }

    // Start is called before the first frame update
    void Start()
    {
       
        healthSlider.value = CaculateSliderPercentage(playerDamageable.Health, playerDamageable.MaxHealth);
        healthBarText.text = "HP" + playerDamageable.Health + " / " + playerDamageable.MaxHealth ;
        
    }
    private void OnEnable()
    {
        playerDamageable.healthChange.AddListener(OnPlayerHealthChange);
    }
    private void OnDisable()
    {
        playerDamageable.healthChange.RemoveListener(OnPlayerHealthChange);
    }


    private float CaculateSliderPercentage(float currentHealth, float maxHealth) 
    {
        return currentHealth / maxHealth;
    }

    private void OnPlayerHealthChange(int newHealth, int maxHealth)
    {

        healthSlider.value = CaculateSliderPercentage(newHealth, maxHealth);
        healthBarText.text = "HP" + newHealth + " / " + maxHealth;

    }

}
