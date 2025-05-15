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

    private void Awake()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;
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

        Destroy(gameObject);
    }
}
