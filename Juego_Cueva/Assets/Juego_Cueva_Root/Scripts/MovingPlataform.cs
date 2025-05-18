using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlataform : MonoBehaviour
{
    public float speed; //Velocidad de la plataforma
    [SerializeField] int startingPoint; //Número para determinar el index del punto de inicio del movimiento
    [SerializeField] Transform[] points; //Array de puntos de posición a los que la plataforma "perseguirá"
    int i; //Index que determina qué número de plataforma se persigue actualmente

    // Start is called before the first frame update
    void Start()
    {
        //Setear la posición inicial de la plataforma en uno de los puntos 
        transform.position = points[startingPoint].position;
    }

    private void FixedUpdate()
    {
        PlatformMove();
    }

    void PlatformMove()
    {
        //Detector de si la plataforma ha llegado al destino, cambiando el destino
        if (Vector3.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++; //Aumenta en uno el index, cambia de objetivo
            if (i == points.Length) i = 0;
        }

        //Movimiento : SIEMPRE DESPUÉS DE LA DETECCION
        //Mueve la plataforma al punto del Array que coincide con el valor de i
        transform.position = Vector3.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }
}
