using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Triggers : MonoBehaviour
{
    public Collider triggerToWatch;       // El trigger que vamos a observar
    public Collider triggerToEnable;      // El trigger que queremos activar después
    private bool previousState = true;    // Guardamos el estado anterior
    private bool hasSwitched = false;     // Para que no se repita el proceso

    void Start()
    {
        triggerToWatch = GetComponent<Collider>();
        if (triggerToWatch != null)
            previousState = triggerToWatch.enabled;
    }

    void Update()
    {
        if (!hasSwitched && previousState && triggerToWatch != null && !triggerToWatch.enabled)
        {
            StartCoroutine(ActivateOtherTriggerAfterDelay(1f));
            hasSwitched = true;
        }

        // Guardar el estado actual para la siguiente comparación
        if (triggerToWatch != null)
            previousState = triggerToWatch.enabled;
    }

    IEnumerator ActivateOtherTriggerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (triggerToEnable != null)
            triggerToEnable.enabled = true;
    }
}
