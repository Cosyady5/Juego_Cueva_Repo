using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health System Configuration")]
    [SerializeField] int maxHealth;
    [SerializeField] int currentHealth;
    private Animator anim;
    private bool isDead = false;
    

    [Header("Respawn Configuration")]
    [SerializeField] Vector3 respawnPoint;
    [SerializeField] List<GameObject> checkPoints;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }
    private void Update()
    {
        if (transform.position.y <= -10) Respawn();
    }
    public void TakeDamage(int enemyDamage)
    {
        if (isDead) return;
        currentHealth -= enemyDamage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            StartCoroutine(Die());
        }
    }
    IEnumerator Die()
    {
        isDead = true;
        anim.SetTrigger("Death");
        yield return new WaitForSecondsRealtime(2f);
        Respawn();
    }

    void Respawn()
    {
        transform.position = respawnPoint;
        currentHealth = maxHealth;
        isDead = false;
        anim.Play("Player_Idle");
    }

    private void OnTriggerEnter(Collider other)
    {
        respawnPoint = transform.position;
        Destroy(other.gameObject);
    }
}
