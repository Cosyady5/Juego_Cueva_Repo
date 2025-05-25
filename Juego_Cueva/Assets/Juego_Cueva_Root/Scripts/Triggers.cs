using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Triggers : MonoBehaviour
{
    public Collider triggerToWatch;      
    public Collider triggerToEnable;      
    private bool previousState = true;   
    private bool hasSwitched = false;     

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
