using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{

    // Configuración de la rotación de la palanca
    public Vector3 rotationOn = new Vector3(-45f, 0f, 0f); // Ángulo cuando se activa
    public Vector3 rotationOff = new Vector3(0f, 0f, 0f);   // Ángulo inicial
    public float rotationSpeed = 2f;
    public bool isOn = false;
    private bool isRotating = false;

    [Header("Objeto a desactivar")]
    // Arrastra el objeto de la jerarquía que quieres desactivar al activar la palanca.
    public GameObject objectToDeactivate;

    void Update()
    {
        // Puedes incluir aquí una comprobación adicional de proximidad si es necesario.
        if (Input.GetKeyDown(KeyCode.E) && !isRotating)
        {
            StartCoroutine(RotateLever());
        }
    }

    IEnumerator RotateLever()
    {
        isRotating = true;
        Quaternion startRotation = transform.localRotation;
        Quaternion endRotation = Quaternion.Euler(isOn ? rotationOff : rotationOn);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * rotationSpeed;
            transform.localRotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        // Cambiamos el estado de la palanca.
        isOn = !isOn;
        isRotating = false;

        // Si la palanca acaba en estado "activado" y hay un objeto asignado, se desactiva.
        if (isOn && objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false);
            Debug.Log("Objeto desactivado por la palanca");
        }
    }

}
