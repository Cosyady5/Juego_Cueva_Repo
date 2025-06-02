using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterfall : MonoBehaviour
{

    public static int points;
    public int winPoints;

    private void Start()
    {
        points = 0;
    }
    void Update()
    {

        if (points >= winPoints)
        {
            gameObject.SetActive(false);
        }
    }
}
