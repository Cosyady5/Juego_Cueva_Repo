using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FracturedObjects : MonoBehaviour
{
    public GameObject original;
    public GameObject fractured;
    //public GameObject vfx;
    public float explosionMinForce = 5;
    public float explosionMaxForce = 100;
    public float explosionForceRadius = 10;
    public float fragScaleFactor = 1;

    private GameObject fractObj;

    public void Explode()
    {
        Collider trigger = GetComponent<Collider>();
        if (trigger != null)
        {
            trigger.enabled = false;
        }
        if (original != null)
        {
            original.SetActive(false);
            if (fractured != null)
            {
                //fractObj = Instantiate(fractured) as GameObject;
                fractObj = Instantiate(fractured, original.transform.position, original.transform.rotation);
                foreach (Transform t in fractObj.transform)
                {
                    var rb = t.GetComponent<Rigidbody>();
                    if (rb != null)
                        rb.AddExplosionForce(Random.Range(explosionMinForce, explosionMaxForce), original.transform.position, explosionForceRadius);

                    StartCoroutine(Shrink(t, 2));
                }

                Destroy(fractObj, 5);

                /*if (vfx != null)
                {
                    GameObject vfx = Instantiate(vfx) as GameObject; ;
                    Destroy(vfx, 7);
                }*/
            }    
        }
    }


    IEnumerator Shrink (Transform t, float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 newScale = t.localScale;
        
        while(newScale.x >= 0)
        {
            if (t == null) yield break;
            newScale -= new Vector3(fragScaleFactor, fragScaleFactor, fragScaleFactor);
            t.localScale = newScale;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
