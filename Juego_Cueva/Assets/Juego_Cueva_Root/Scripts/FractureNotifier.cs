using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureNotifier : MonoBehaviour
{
    public Crystal crystal;
    public void ExplodeWithNotify()
    {
        if (crystal != null)
        {
           // crystal.MarkAsDestroyed();
        }

        // Llamamos al script original de fractura
        FracturedObjects fracture = GetComponent<FracturedObjects>();
        if (fracture != null)
        {
            fracture.Explode();
        }
    }
}
