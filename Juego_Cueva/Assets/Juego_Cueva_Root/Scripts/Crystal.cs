using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public CrystalManager crystalManager;

    private void Start()
    {
        if (crystalManager == null)
        {
            crystalManager = FindObjectOfType<CrystalManager>();
        }

        crystalManager?.RegisterCrystal(this);
    }
    private void OnDisable()
    {
        crystalManager?.UnregisterCrystal(this);
    }
}
