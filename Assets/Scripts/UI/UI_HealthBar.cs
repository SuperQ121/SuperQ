using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    [SerializeField] private CharacterStat myStats;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI healthText;
    void Start()
    {
        slider =GetComponent<Slider>();
        myStats =GetComponentInParent<CharacterStat>();

        myStats.onHealthChanged += UpdateHealthUI;
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.maxHealth.GetValue();
        slider.value = myStats.currentHealth;
        healthText.text = myStats.currentHealth + "/" + myStats.maxHealth.GetValue();
    }
}
