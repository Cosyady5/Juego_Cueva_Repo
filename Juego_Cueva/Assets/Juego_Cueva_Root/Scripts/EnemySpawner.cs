using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public List<Transform> spawnPoints;
    public float spawnInterval = 3f;
    public Collider triggerZone; // Zona de entrada del jugador
    public int maxEnemiesAlive = 5;
    public bool hasboss = false;
    public GameObject boss;
    public GameObject hpBoss;
    //[Header("Crystal Settings")]
    //public int totalCrystals = 4;
    //private int destroyedCrystals = 0;

    [SerializeField] private bool canSpawn = false;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private int currentSpawnIndex = 0;


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

            spawnedEnemies.RemoveAll(e => e == null); //new

            //if (spawnPoints.Count <= 0) yield break;
            if (spawnedEnemies.Count < maxEnemiesAlive)
            {

                int index = currentSpawnIndex % spawnPoints.Count;
                currentSpawnIndex++;

                Transform selectedPoint = spawnPoints[index];
                GameObject enemy = Instantiate(enemyPrefab, selectedPoint.position, selectedPoint.rotation);
                spawnedEnemies.Add(enemy);
                yield return new WaitForSeconds(spawnInterval);

                EnemyHealth health = enemy.GetComponent<EnemyHealth>();
                if (health != null) health.SetSpawner(this);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void StopSpawning()
    {
        canSpawn = false;

        // Matar a todos los enemigos ya generados
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    // Solo iniciar la muerte si no está ya muriendo
                    if (!enemyHealth.GetIsDead())
                    {
                        StartCoroutine(enemyHealth.Die());
                        StartCoroutine(enemyHealth.DissolveCo());
                    }
                }
            }
            triggerZone.enabled = false;
        }

        spawnedEnemies.Clear();

        if (hasboss)
        {
            boss.SetActive(true);
            hpBoss.SetActive(true);
        }

    }
    public void NotifyEnemyDied(GameObject enemy)
    {
        spawnedEnemies.Remove(enemy);
    }

}
