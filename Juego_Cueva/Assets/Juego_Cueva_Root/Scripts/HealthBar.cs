using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private Image healthbarSprite;
    
    public void UpdateHealthBar(float maxHealth, float currentHeatlh)
    {
        healthbarSprite.fillAmount = currentHeatlh / maxHealth;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
