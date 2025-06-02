using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health System Configuration")]
    [SerializeField] private HealthBar healthBar;
    [SerializeField] int maxHealth;
    [SerializeField] int currentHealth;
    private Animator anim;
    private bool isDead = false;
    private EnemySpawner spawner;

    [Header("Feedback Configuration")]
    public SkinnedMeshRenderer skinnedMesh;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;
    private Material[] skinnedMaterials;
    [SerializeField] GameObject deathEffect;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        if (skinnedMesh != null)
            skinnedMaterials = skinnedMesh.materials;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
        StartCoroutine(FlashRed());
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            StartCoroutine(Die());
            healthBar.gameObject.SetActive(false);
            if (skinnedMaterials.Length > 0) StartCoroutine(DissolveCo());
        }
    }

    public IEnumerator Die()
    {
        isDead = true;
        anim.SetTrigger("Death");
        // Espera hasta que termine la animación de muerte
        yield return new WaitForSecondsRealtime(2f);

        spawner?.NotifyEnemyDied(gameObject);
        Destroy(gameObject);
    }

    public IEnumerator DissolveCo()
    {
        deathEffect.SetActive(true);
        float counter = 0;
        while (skinnedMaterials[0].GetFloat("_DissolveAmount") < 1)
        {
            counter += dissolveRate;
            for (int i = 0; i < skinnedMaterials.Length; i++)
            {
                skinnedMaterials[i].SetFloat("_DissolveAmount", counter);
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }

    IEnumerator FlashRed()
    {
        foreach (var mat in skinnedMesh.materials)
        {
            mat.SetColor("_BaseColor", Color.red);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (var mat in skinnedMesh.materials)
        {
            mat.SetColor("_BaseColor", Color.white);
        }
    }
    public bool GetIsDead()
    {
        return isDead;
    }
    public void SetSpawner(EnemySpawner s)
    {
        spawner = s;
    }
}