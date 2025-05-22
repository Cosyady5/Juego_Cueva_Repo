using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShooter : MonoBehaviour
{

    public GameObject arrowPrefab;
    public Transform shootPoint;
    public float shootForce = 20f;
    public float shootInterval = 2f;
    
    void Start()
    {
        InvokeRepeating(nameof(ShootArrow), 1f, shootInterval);
    }

    void ShootArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.velocity = shootPoint.forward * shootForce;
    }

}
