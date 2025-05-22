using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Haz daño al jugador
        }

        Destroy(gameObject, 2f); // Desaparece tras impactar
    }

}
