using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 3f;
    public Collider triggerZone; // Zona de entrada del jugador

    [Header("Crystal Settings")]
    public int totalCrystals = 4;

    private int destroyedCrystals = 0;
    private bool canSpawn = false;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private void Start()
    {
        // Asegúrate de que el trigger esté en modo "IsTrigger"
        if (triggerZone != null)
        {
            triggerZone.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !canSpawn)
        {
            canSpawn = true;
            StartCoroutine(SpawnLoop());
            Debug.Log("Jugador ha entrado en la sala. Spawner activado.");
        }
    }

    IEnumerator SpawnLoop()
    {
        while (canSpawn)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            spawnedEnemies.Add(enemy);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Llamar esto desde el cristal cuando se destruya
    public void NotifyCrystalDestroyed()
    {
        destroyedCrystals++;

        if (destroyedCrystals >= totalCrystals)
        {
            StopSpawning();
        }
    }

    public void StopSpawning()
    {
        canSpawn = false;
        Debug.Log("Todos los cristales destruidos. Spawner desactivado.");

        // Matar a todos los enemigos ya generados
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy); // Puedes reemplazar esto con enemy.GetComponent<EnemyHealth>().Die() si tienes animación
            }
        }

        spawnedEnemies.Clear();
    }

}
