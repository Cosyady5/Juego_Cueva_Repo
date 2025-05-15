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
    public SkinnedMeshRenderer skinnedMesh;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;
    private Animator anim;
    private bool isDead = false;
    //public GameObject particles;

    private Material[] skinnedMaterials;
    //[Header("Feedback Configuration")]
    //[SerializeField] Material baseMat;
    //[SerializeField] Material damagedMat;
    [SerializeField] GameObject deathEffect;
    [SerializeField] Collider attackCollider;
    // Autorrefernecias privadas
    //MeshRenderer enemyRend;

    private void Awake()
    {
        //enemyRend = GetComponent<MeshRenderer>();
        //baseMat = enemyRend.material;
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
        //enemyRend.material = damagedMat;
        //Invoke(nameof(ResetDamageMeterial), 0.2f);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            StartCoroutine(Die());
            healthBar.gameObject.SetActive(false);
            deathEffect.SetActive(true);
            if (skinnedMaterials.Length > 0) StartCoroutine(DissolveCo());
        }
    }
    public void EnableAttackCollider()
    {
        attackCollider.enabled = true;
    }

    public void DisableAttackCollider()
    {
        attackCollider.enabled = false;
    }

    IEnumerator Die()
    {
        isDead = true;
        anim.SetTrigger("Death");
        // Espera hasta que termine la animación de muerte
        yield return new WaitForSecondsRealtime(2f);

        Destroy(gameObject);
    }

    IEnumerator DissolveCo()
    {
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
    /*private void ResetDamageMeterial()
    {
        enemyRend.material = baseMat;
    }*/
}