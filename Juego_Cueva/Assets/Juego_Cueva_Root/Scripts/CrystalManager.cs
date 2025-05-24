using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalManager : MonoBehaviour
{

    //public EnemySpawner[] spawnersToDisable;
    public EnemySpawner spawnerToDisable;
    private List<Crystal> registeredCrystals = new List<Crystal>();

    public void RegisterCrystal(Crystal crystal)
    {
        if (!registeredCrystals.Contains(crystal))
            registeredCrystals.Add(crystal);
    }

    public void UnregisterCrystal(Crystal crystal)
    {
        if (registeredCrystals.Contains(crystal))
        {
            registeredCrystals.Remove(crystal);
            CheckCrystals();
        }
    }

    private void CheckCrystals()
    {
        if (registeredCrystals.Count == 0)
        {
            StopAllSpawners();
        }
    }

    /*private void StopAllSpawners()
    {
        foreach (var spawner in spawnersToDisable)
        {
            spawner.StopSpawning();
        }
    }*/
    private void StopAllSpawners()
    {
        if (spawnerToDisable != null)
        {
            spawnerToDisable.StopSpawning();
        }
    }
}
