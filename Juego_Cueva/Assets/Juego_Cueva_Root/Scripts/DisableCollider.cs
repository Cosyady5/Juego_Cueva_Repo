using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCollider : MonoBehaviour
{
    public List<GameObject> objects;
    [SerializeField] private bool isobjects;
    [SerializeField] private bool isenemy;

    void Update()
    {
        if (isobjects)
        {
            bool allInactive = true;
            foreach (GameObject obj in objects)
            {
                if (obj != null && obj.activeSelf)
                {
                    allInactive = false;
                    break;
                }
            }

            if (allInactive && gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }
        if (isenemy)
        {
            objects.RemoveAll(obj => obj == null);

            if (objects.Count == 0 && gameObject.activeSelf)
            {
                gameObject.SetActive(false);
                enabled = false;
            }
        }
    }
}
