using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    public GameObject heartPrefab;
    public Transform heartsContainer;

    private List<Image> hearts = new List<Image>();
    private bool initialized = false;

    public void UpdateHearts(int currentHealth)
    {
        if (!initialized)
        {
            // Buscamos al PlayerHealth para saber cuántos corazones crear
            PlayerHealth player = FindObjectOfType<PlayerHealth>();
            if (player == null)
            {
                Debug.LogError("No se encontró PlayerHealth en la escena.");
                return;
            }

            int maxHearts = player.GetMaxHealth();

            for (int i = 0; i < maxHearts; i++)
            {
                GameObject heart = Instantiate(heartPrefab, heartsContainer);
                Image heartImage = heart.GetComponent<Image>();
                hearts.Add(heartImage);
            }

            initialized = true;
        }

        // Actualizamos los corazones activos e inactivos
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].color = Color.white;
            }
            else
            {
                Color inactiveColor;
                ColorUtility.TryParseHtmlString("#727272B3", out inactiveColor);
                hearts[i].color = inactiveColor;
            }
        }
    }
}
