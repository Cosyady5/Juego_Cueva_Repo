using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health System Configuration")]
    [SerializeField] int maxHealth;
    [SerializeField] int currentHealth;
    private Animator anim;
    private bool isDead = false;
    public HeartUI uiManager;

    [Header("Respawn Configuration")]
    [SerializeField] Vector3 respawnPoint;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        uiManager.UpdateHearts(currentHealth);
        respawnPoint = transform.position;
    }
    private void Update()
    {
        if (transform.position.y <= -10) Respawn();
    }


    public void TakeDamage(int enemyDamage)
    {
        if (isDead) return;
        currentHealth -= enemyDamage;
        uiManager.UpdateHearts(currentHealth);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            StartCoroutine(Die());
        }
    }
    IEnumerator Die()
    {
        GetComponent<PlayerController>().isDead = true;
        isDead = true;
        anim.SetTrigger("Death");

        // Disable physics temporarily
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;

        yield return new WaitForSecondsRealtime(2f);

        Respawn();

        // Re-enable physics
    }

    void Respawn()
    {
        //Vector3 preTeleportPos = transform.position;
        //GetComponent<PlayerController>().enabled = false;

        transform.position = respawnPoint;
        transform.rotation = Quaternion.identity;

        Physics.SyncTransforms();

        // Reset states
        GetComponent<PlayerController>().isDead = false;
        currentHealth = maxHealth;
        isDead = false;

        anim.Play("Player_Idle", 0, 0);

        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        uiManager.UpdateHearts(currentHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            StartCoroutine(Die());
        }

        if (other.CompareTag("Checkpoint"))
        {
            AudioManager.Instance.PlaySFX(4);
            respawnPoint = transform.position;
            Destroy(other.gameObject);
        }
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
