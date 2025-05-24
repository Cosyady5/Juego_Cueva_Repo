using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject objectToActivate; // Objeto que se activará
    public GameObject nextTrigger;
    public float activeTime = 3f;       // Tiempo en segundos que permanecerá activo
    private Collider triggerCollider;   // Referencia al collider del trigger

    void Start()
    {
        triggerCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && triggerCollider.enabled)
        {
            StartCoroutine(ActivateAndDisable());
        }

        if (nextTrigger != null)
        {
            nextTrigger.SetActive(true);
        }
    }

    IEnumerator ActivateAndDisable()
    {
        // Activar el objeto
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }

        // Esperar el tiempo indicado
        yield return new WaitForSeconds(activeTime);

        // Desactivar el objeto
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(false);
        }

        // Desactivar el trigger para que no se vuelva a activar
        triggerCollider.enabled = false;
    }
}

