using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolvingController : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;
    public Animator anim;
    public GameObject particles;

    private Material[] skinnedMaterials;
    void Start()
    {
        if (skinnedMesh != null)
            skinnedMaterials = skinnedMesh.materials;
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
    public void Death()
    {
        particles.SetActive(true);
        anim.SetBool("Death", true);
        if (skinnedMaterials.Length > 0) StartCoroutine(DissolveCo());
    }
    public void ResetEffect()
    {
        particles.SetActive(false);
        anim.SetBool("Death", false);
        for (int i = 0; i < skinnedMaterials.Length; i++)
            {
                skinnedMaterials[i].SetFloat("_DissolveAmount", 0);
            }
    }
}