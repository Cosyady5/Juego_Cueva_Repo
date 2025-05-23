using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterfall : MonoBehaviour
{

    public static int points;
    public int winPoints;
  

    // Update is called once per frame
    void Update()
    {

        if (points >= winPoints)
        {
            Destroy(gameObject);
        }
    }
}
