using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{

    public static int totalCrystals = 4; // Total de cristales en la escena
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
    }

}
