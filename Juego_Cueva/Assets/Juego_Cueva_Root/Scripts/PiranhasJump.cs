using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhasJump : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float jumpDuration = 1f;
    [SerializeField] private float delayBetweenJumps = 1.5f;

    [Header("Movement Settings")]
    [SerializeField] private float forwardDistance = 7f; // Distancia hacia adelante

    private Vector3 startPos;
    private Quaternion startRot;

    private void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        InvokeRepeating(nameof(StartJump), 0f, delayBetweenJumps);
    }

    private void StartJump()
    {
        StartCoroutine(ParabolicJump());
    }

    private IEnumerator ParabolicJump()
    {
        float elapsed = 0f;
        Vector3 start = startPos;
        Vector3 end = startPos + transform.forward * forwardDistance;

        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / jumpDuration;

            // Interpolación en X, Y y Z para que siga la parábola en el espacio 3D
            float x = Mathf.Lerp(start.x, end.x, t);
            float y = start.y + 4 * jumpHeight * t * (1 - t);
            float z = Mathf.Lerp(start.z, end.z, t);

            transform.position = new Vector3(x, y, z);

            // Rotar en X para simular la inclinación del salto
            float angleX = Mathf.Lerp(-60f, 60f, t);
            // Mantener la rotación Y y Z originales (startRot), solo modificar X
            Quaternion rotationX = Quaternion.Euler(angleX, 0f, 0f);
            transform.rotation = startRot * rotationX;

            yield return null;
        }

        // Volver a posición y rotación iniciales
        transform.position = startPos;
        transform.rotation = startRot;
    }
}
