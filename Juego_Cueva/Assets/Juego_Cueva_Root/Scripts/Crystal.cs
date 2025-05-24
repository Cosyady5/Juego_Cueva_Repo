using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    /*
    public static int totalCrystals = 4;
    private static int destroyedCrystals = 0;
    public EnemySpawner[] spawnersToDisable;

    public void DestroyCrystal()
    {
        destroyedCrystals++;
        Destroy(gameObject);

        if (destroyedCrystals >= totalCrystals)
        {
            StopAllSpawners();
        }
    }

    void StopAllSpawners()
    {
        foreach (EnemySpawner spawner in spawnersToDisable)
        {
            spawner.StopSpawning();
        }

        Debug.Log("Todos los cristales han sido destruidos. Spawns detenidos.");
    }*/
    public CrystalManager crystalManager;

    private bool isDestroyed = false;

    private void Start()
    {
        if (crystalManager == null)
        {
            crystalManager = FindObjectOfType<CrystalManager>();
        }

        crystalManager?.RegisterCrystal(this);
    }

    /*private void OnDestroy()
    {
        if (isDestroyed) crystalManager?.UnregisterCrystal(this);
    }
    
    public void MarkAsDestroyed()
    {
        isDestroyed = true;
    }*/
    private void OnDisable()
    {
        crystalManager?.UnregisterCrystal(this);
    }
}
